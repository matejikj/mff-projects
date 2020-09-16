package com.matejik.relevantly.fragments;

import android.content.Intent;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout;

import com.matejik.relevantly.models.Article;
import com.matejik.relevantly.R;
import com.matejik.relevantly.RssFeedListAdapter;

import org.xmlpull.v1.XmlPullParser;
import org.xmlpull.v1.XmlPullParserException;
import org.xmlpull.v1.XmlPullParserFactory;

import java.io.IOException;
import java.io.InputStream;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

/**
 * Fragment with list of selected articles
 */
public class SourceFragment extends Fragment {
    private String urlLink;
    private RecyclerView mRecyclerView;
    private SwipeRefreshLayout mSwipeLayout;
    private List<Article> articles;
    public static final String itemTag = "item";
    public static final String titleTag = "title";
    public static final String linkTag = "link";
    public static final String descriptionTag = "description";


    public SourceFragment(String arg){
        urlLink = arg;
    }

    public String getSourceName() {
        String newUrl = urlLink.replaceFirst("^(http[s]?://www\\.|http[s]?://|www\\.)","");
        int n = newUrl.indexOf('.');
        if (n > 0){
            return newUrl.substring(0,n);
        }
        else{
            return newUrl;
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View rootView = inflater.inflate( R.layout.fragment_source_viewer, container, false);
        mRecyclerView = rootView.findViewById(R.id.recyclerView);
        mSwipeLayout = rootView.findViewById(R.id.swipeRefreshLayout);
        mRecyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));
        mSwipeLayout.setOnRefreshListener(() -> new FetchFeedTask().execute((Void) null));
        new FetchFeedTask().execute((Void) null);
        return rootView;
    }

    /**
     * Parse xml inputstream
     */
    public List<Article> parseFeed(InputStream inputStream) throws XmlPullParserException, IOException {
        String title = null;
        String link = null;
        String description = null;
        boolean isInItem;
        List<Article> items = new ArrayList<>();
        int eventType;

        try {
            XmlPullParserFactory factory = XmlPullParserFactory.newInstance();
            factory.setNamespaceAware(false);
            XmlPullParser xpp = factory.newPullParser();
            xpp.setInput(inputStream, "UTF_8");
            isInItem = false;
            eventType = xpp.getEventType();
            while (eventType != XmlPullParser.END_DOCUMENT)
            {
                if (eventType == XmlPullParser.START_TAG)
                {
                    if (xpp.getName().equalsIgnoreCase(itemTag))
                    {
                        isInItem = true;
                    }
                    else if (xpp.getName().equalsIgnoreCase(titleTag))
                    {
                        if (isInItem)
                        {
                            title = xpp.nextText();
                        }
                    }
                    else if (xpp.getName().equalsIgnoreCase(linkTag))
                    {
                        if (isInItem)
                        {
                            link = xpp.nextText();
                        }
                    }
                    else if (xpp.getName().equalsIgnoreCase(descriptionTag))
                    {
                        if (isInItem)
                        {
                            description = xpp.nextText();
                        }
                    }
                }
                else if (eventType == XmlPullParser.END_TAG && xpp.getName().equalsIgnoreCase(itemTag))
                {
                    isInItem = false;
                    Article article = new Article();
                    if (title != null){
                        article.setTitle(title);
                    }
                    if (description != null){
                        article.setDescription(description);
                    }
                    if (link != null){
                        article.setLink(link);
                    }
                    items.add(article);
                    title = null;
                    description = null;
                    link = null;
                }

                eventType = xpp.next();
            }
        }
        finally {
            inputStream.close();
        }
        return items;
    }

    /**
     * Class for async fetching sources
     */
    private class FetchFeedTask extends AsyncTask<Void, Void, Boolean> {

        @Override
        protected void onPreExecute() {
            mSwipeLayout.setRefreshing(true);
        }

        @Override
        protected Boolean doInBackground(Void... voids) {
            if (TextUtils.isEmpty(urlLink)){
                return false;
            }
            try {
                if (!urlLink.startsWith("http://") && !urlLink.startsWith("https://")){
                    urlLink = "http://" + urlLink;
                }
                URL url = new URL(urlLink);
                InputStream inputStream = url.openConnection().getInputStream();
                articles = parseFeed(inputStream);
                return true;
            } catch (IOException e) {
                Log.e(this.getClass().getName(), "Error", e);
            } catch (XmlPullParserException e) {
                Log.e(this.getClass().getName(), "Error", e);
            }
            return false;
        }

        @Override
        protected void onPostExecute(Boolean success) {
            mSwipeLayout.setRefreshing(false);

            if (success) {
                RssFeedListAdapter adapter = new RssFeedListAdapter(articles);
                adapter.setOnClick(position -> {
                    Intent i = new Intent(Intent.ACTION_VIEW);
                    i.setData(Uri.parse(articles.get(position).getLink()));
                    getActivity().startActivity(i);
                });
                mRecyclerView.setAdapter(adapter);
            }
        }
    }


}
