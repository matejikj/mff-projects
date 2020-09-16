package com.matejik.relevantly.activities;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.google.gson.Gson;
import com.matejik.relevantly.R;
import java.util.ArrayList;

import butterknife.BindView;
import butterknife.ButterKnife;

public class SourceActivity extends AppCompatActivity {

    @BindView(R.id.toolbarSource)
    Toolbar toolbar;

    @BindView(R.id.sourceListView)
    ListView sourceListView;

    @BindView(R.id.nav_view)
    BottomNavigationView navView;

    public static ArrayList<String> urls = new ArrayList<>();
    public static ArrayAdapter arrayAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_source);

        ButterKnife.bind(this);

        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Relevantly");

        navView.setSelectedItemId(R.id.navigation_source);
        navView.setOnNavigationItemSelectedListener(item -> {
            switch (item.getItemId()) {
                case R.id.navigation_reader:
                    Intent a = new Intent(SourceActivity.this, ReaderActivity.class);
                    startActivity(a);
                    break;
                case R.id.navigation_source:
                    Intent b = new Intent(SourceActivity.this, SourceActivity.class);
                    startActivity(b);
                    break;
            }
            return false;
        });

        SharedPreferences sharedPreferences = getApplicationContext().getSharedPreferences("com.matejik.relevantly", Context.MODE_PRIVATE);
        Gson gson = new Gson();

        String jsonText = sharedPreferences.getString("urls", null);
        ArrayList<String> set = gson.fromJson(jsonText, ArrayList.class);

        if (set == null){
            if (urls.size() ==  0)
                urls.add("Edit this url");
        } else {
            urls = new ArrayList<>(set);
        }


        arrayAdapter = new ArrayAdapter(this, android.R.layout.simple_list_item_1, urls);
        sourceListView.setAdapter(arrayAdapter);
        sourceListView.setOnItemClickListener((adapterView, view, i, l) -> {
            Intent intent = new Intent(getApplicationContext(), SourceEditActivity.class);
            intent.putExtra("urlId", i);
            startActivity(intent);
        });

        sourceListView.setOnItemLongClickListener((adapterView, view, i, l) -> {

            int itemId = i;

            new AlertDialog.Builder(SourceActivity.this)
                    .setMessage(getResources().getString(R.string.alert_dialog_message))
                    .setPositiveButton(getResources().getString(R.string.yes), (dialogInterface, i1) -> {
                        urls.remove(itemId);
                        arrayAdapter.notifyDataSetChanged();
                        String stringSet = gson.toJson(SourceActivity.urls);
                        sharedPreferences.edit().putString("urls", stringSet).apply();
                    })
                    .setNegativeButton(getResources().getString(R.string.no), null)
                    .show();
            return true;
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        getMenuInflater().inflate(R.menu.source_menu, menu);
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item){
        switch (item.getItemId()){
            case R.id.toolbar_addNew:
                Intent a = new Intent(getApplicationContext(), SourceEditActivity.class);
                startActivity(a);
                break;
            case R.id.toolbar_about:
                Intent b = new Intent(getApplicationContext(), AboutActivity.class);
                startActivity(b);
                break;
            case R.id.toolbar_refresh:
                Intent c = new Intent(getApplicationContext(), ReaderActivity.class);
                startActivity(c);
                break;
        }
        return super.onOptionsItemSelected(item);
    }
}
