package com.matejik.relevantly.activities;

import android.content.Intent;
import android.os.Bundle;
import com.matejik.relevantly.R;
import androidx.appcompat.app.AppCompatActivity;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.ImageView;

/**
 * Show animation and launch readerActivity
 */
public class IntroActivity extends AppCompatActivity {

    /**
     * Initialize main activity on start, started with animation and start topicsactivity
     */
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_intro);

        ImageView introLogo = findViewById(R.id.imageView);
        Animation animation = AnimationUtils.loadAnimation(this, R.anim.intro);
        introLogo.startAnimation(animation);

        Thread thread = new Thread() {
            public void run() {
                try {
                    sleep(600);
                    startActivity(new Intent(getApplicationContext(), ReaderActivity.class));
                } catch (InterruptedException e) {
                    e.printStackTrace();
                } finally {
                    finish();
                }
            }
        };
        thread.start();
    }
}
