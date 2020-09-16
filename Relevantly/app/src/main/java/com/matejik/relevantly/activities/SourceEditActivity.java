package com.matejik.relevantly.activities;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.EditText;
import android.widget.TextView;

import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.google.gson.Gson;
import com.matejik.relevantly.R;
import butterknife.BindView;
import butterknife.ButterKnife;

public class SourceEditActivity extends AppCompatActivity {
    int urlId;

    @BindView(R.id.editUrlText)
    EditText editUrlText;

    @BindView(R.id.titleUrlText)
    TextView titleUrlText;

    @BindView(R.id.toolbar)
    Toolbar toolbar;

    @BindView(R.id.save_edit)
    FloatingActionButton fab;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_source_edit);

        ButterKnife.bind(this);

        titleUrlText.setText(getResources().getString(R.string.insert_url_text));

        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Relevantly");

        Intent intent = getIntent();
        urlId = intent.getIntExtra("urlId", -1);

        if (urlId != -1 ){
            editUrlText.setText(SourceActivity.urls.get(urlId));
        }
        else{
            SourceActivity.urls.add("");
            urlId = SourceActivity.urls.size() - 1;
            SourceActivity.arrayAdapter.notifyDataSetChanged();
        }

        fab.setOnClickListener(view -> finish());

        editUrlText.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {

            }

            @Override
            public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
                SourceActivity.urls.set(urlId, charSequence.toString());
                SourceActivity.arrayAdapter.notifyDataSetChanged();
                SharedPreferences sharedPreferences = getApplicationContext().getSharedPreferences("com.matejik.relevantly", MODE_PRIVATE);
                Gson gson = new Gson();
                String stringSet = gson.toJson(SourceActivity.urls);
                sharedPreferences.edit().putString("urls", stringSet).apply();
            }

            @Override
            public void afterTextChanged(Editable editable) {
            }
        });
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
                Intent c = new Intent(SourceEditActivity.this, AboutActivity.class);
                startActivity(c);
                break;
        }
        return super.onOptionsItemSelected(item);
    }
}
