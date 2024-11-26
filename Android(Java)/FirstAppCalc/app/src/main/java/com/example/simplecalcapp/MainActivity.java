package com.example.simplecalcapp;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class MainActivity extends AppCompatActivity {

    TextView result;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        result = findViewById(R.id.textView);
        result.setText("");

    }

    public void numberAndOperatorFunc(View view) {

        Button button = (Button) view;
        result.append(button.getText().toString());
    }

    public void equalsFunc(View view) {

        String str = result.getText().toString();

        String[] parts = str.split("(?=[\\+\\-\\*/])|(?<=[\\+\\-\\*/])");

        if (parts.length == 3) {
            int num1 = Integer.parseInt(parts[0].trim());
            int num2 = Integer.parseInt(parts[2].trim());
            char operator = parts[1].charAt(0);
            int finalCalc = 0;

            switch (operator) {
                case '+':
                    finalCalc = num1 + num2;
                    break;
                case '-':
                    finalCalc = num1 - num2;
                    break;
                case '*':
                    finalCalc = num1 * num2;
                    break;
                case '/':
                    finalCalc = num1 / num2;
                    break;
            }

            result.setText(String.valueOf(finalCalc));
        }
    }



    public void clearFunc(View view) {
        result.setText("");
    }
}



