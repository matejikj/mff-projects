package com.matejik.relevantly.activities;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.viewpager.widget.PagerAdapter;
import androidx.viewpager.widget.ViewPager;
import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.google.gson.Gson;
import com.matejik.relevantly.fragments.SourceFragment;
import com.matejik.relevantly.PagesPagerAdapter;
import com.matejik.relevantly.R;
import java.util.ArrayList;
import butterknife.BindView;
import butterknife.ButterKnife;

/**
 * Class for activity with newspapers
 */
public class ReaderActivity extends AppCompatActivity {

    ArrayList<String> sources;

    @BindView(R.id.toolbar)
    Toolbar toolbar;

    @BindView(R.id.nav_view)
    BottomNavigationView navView;

    @BindView(R.id.pagerReader)
    ViewPager pagerReader;

    private PagerAdapter pagerAdapter;

    /**
     * Create new acitivty with newspapers
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_reader);

        ButterKnife.bind(this);

        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Relevantly");

        navView.setSelectedItemId(R.id.navigation_reader);
        navView.setOnNavigationItemSelectedListener(item -> {
            switch (item.getItemId()) {
                case R.id.navigation_reader:
                    Intent a = new Intent(ReaderActivity.this, ReaderActivity.class);
                    startActivity(a);
                    break;
                case R.id.navigation_source:
                    Intent b = new Intent(ReaderActivity.this, SourceActivity.class);
                    startActivity(b);
                    break;
            }
            return false;
        });

        pagerAdapter = new PagesPagerAdapter(getSupportFragmentManager());
        SharedPreferences sharedPreferences = getApplicationContext().getSharedPreferences("com.matejik.relevantly", Context.MODE_PRIVATE);
        Gson gson = new Gson();
        String jsonText = sharedPreferences.getString("urls", null);
        ArrayList<String> set = gson.fromJson(jsonText, ArrayList.class);

        if (set == null){
            sources = new ArrayList<>();
            sources.add("Edit first in Source");
        } else {
            sources = new ArrayList<>(set);
        }

        for (String source : sources ){
            ((PagesPagerAdapter) pagerAdapter).addFrag(new SourceFragment(source));
        }
        pagerReader.setAdapter(pagerAdapter);
    }

    /**
     * When pressed onback
     */
    @Override
    public void onBackPressed() {
        if (pagerReader.getCurrentItem() == 0) {
            super.onBackPressed();
        } else {
            pagerReader.setCurrentItem(pagerReader.getCurrentItem() - 1);
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        getMenuInflater().inflate(R.menu.main_menu, menu);
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item){
        switch (item.getItemId()){
            case R.id.toolbar_about:
                Intent c = new Intent(ReaderActivity.this, AboutActivity.class);
                startActivity(c);
                break;
        }
        return super.onOptionsItemSelected(item);
    }
}
