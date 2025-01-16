package com.example.applicationmemogame;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageButton;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.Random;
import java.util.Timer;
import java.util.TimerTask;

public class MainActivity extends Activity {

    ArrayList<Integer> arr_image;
    ArrayList<ImageButtonClass> arr_object;
    int z=0,x,y,count=0;
    ImageButton x_Button;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        arr_image = new ArrayList<>();
        arr_object = new ArrayList<>();

        for(int j=0 ; j<2 ; j++)
            for(int i=1 ; i<7 ; i++)
                arr_image.add(getResources().getIdentifier("ex"+i, "drawable", getPackageName()));

        Collections.shuffle(arr_image);
    }

    public void func_button(View view) {

        int id_button;

        if(view instanceof ImageButton) {
            final ImageButton imageButton = (ImageButton) view;

            if((id_button = findIdButton(view.getId())) == -1 ) {
                arr_object.add(new ImageButtonClass(arr_image.get(z), view.getId()));

                imageButton.setImageResource(arr_image.get(z));
                z++;

            }
            else {
                imageButton.setImageResource(arr_object.get(id_button).getId());
            }

            if(count == 0) {
                x = arr_object.get(findIdButton(view.getId())).getId();
                x_Button = imageButton;
                count++;
            }
            else
            {
                y= arr_object.get(findIdButton(view.getId())).getId();
                count=0;
                if(x == y && x_Button != view) {
                    Toast.makeText(this,"Yes! you found a match!",Toast.LENGTH_LONG).show();
                    view.setVisibility(view.GONE);
                    x_Button.setVisibility(view.GONE);
                }
            }

            Timer buttonTimer = new Timer();
            buttonTimer.schedule(new TimerTask() {
                @Override
                public void run() {
                    runOnUiThread(new Runnable() {

                        @Override
                        public void run() {

                            imageButton.setImageResource(R.mipmap.imagefound);
                        }
                    });
                }
            }, 3000);
        }
    }

    private int findIdButton(int id) {

        int i=0;

        for(ImageButtonClass image : arr_object)
        {
            if(image.getArrId() == id)

                return i;
            i++;
        }
        return -1;
    }
}