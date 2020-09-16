package com.matejik.relevantly;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import androidx.recyclerview.widget.RecyclerView;
import com.matejik.relevantly.models.Article;
import java.util.List;

public class RssFeedListAdapter extends RecyclerView.Adapter<RssFeedListAdapter.FeedModelViewHolder> {

    OnItemClicked onClick;

    private List<Article> mRssFeedModels;

    public static class FeedModelViewHolder extends RecyclerView.ViewHolder {
        private View rssFeedView;
        public FeedModelViewHolder(View v) {
            super(v);
            rssFeedView = v;
        }
    }

    public RssFeedListAdapter(List<Article> rssFeedModels) {
        mRssFeedModels = rssFeedModels;
    }

    @Override
    public FeedModelViewHolder onCreateViewHolder(ViewGroup parent, int type) {
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_listview, parent, false);
        FeedModelViewHolder holder = new FeedModelViewHolder(v);
        return holder;
    }

    @Override
    public void onBindViewHolder(FeedModelViewHolder holder, final int position) {
        final Article rssFeedModel = mRssFeedModels.get(position);
        ((TextView)holder.rssFeedView.findViewById(R.id.titleText)).setText(rssFeedModel.getTitle());
        ((TextView)holder.rssFeedView.findViewById(R.id.descriptionText)).setText(rssFeedModel.getDescription());

        holder.itemView.setOnClickListener(v -> onClick.onItemClick(position));
    }

    @Override
    public int getItemCount() {
        return mRssFeedModels.size();
    }

    public void setOnClick(OnItemClicked onClick)
    {
        this.onClick = onClick;
    }
}
