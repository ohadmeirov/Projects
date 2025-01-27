package com.example.MyTrip.activities;

import android.os.Bundle;
import android.widget.EditText;
import android.widget.Toast;
import com.example.MyTrip.interfaces.LoginCallback;
import com.example.MyTrip.interfaces.RegisterCallback;
import androidx.activity.EdgeToEdge;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.MyTrip.R;
import com.example.MyTrip.models.Student;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.AuthResult;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.database.DataSnapshot;
import com.google.firebase.database.DatabaseError;
import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;
import com.google.firebase.database.ValueEventListener;

public class MainActivity extends AppCompatActivity {

    private FirebaseAuth mAuth;
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
        mAuth = FirebaseAuth.getInstance();
    }


    public void register(RegisterCallback callback) {
        String email = ((EditText) findViewById(R.id.editTextEmailAddressAtRegisterPage)).getText().toString();
        String password = ((EditText) findViewById(R.id.editTextPasswordAtRegisterPage)).getText().toString();

        mAuth.createUserWithEmailAndPassword(email, password)
                .addOnCompleteListener(this, new OnCompleteListener<AuthResult>() {
                    @Override
                    public void onComplete(@NonNull Task<AuthResult> task) {
                        if (task.isSuccessful()) {
                            Toast.makeText(MainActivity.this, "reg ok", Toast.LENGTH_LONG).show();
                            callback.onRegisterSuccess();
                        } else {
                            Toast.makeText(MainActivity.this, "reg fail", Toast.LENGTH_LONG).show();
                            callback.onRegisterFail();
                        }
                    }
                });
    }

    public void login(LoginCallback callback) {
        String email = ((EditText) findViewById(R.id.editTextEmailAddressAtMainPage)).getText().toString();
        String password = ((EditText) findViewById(R.id.editTextPasswordAtMainPage)).getText().toString();

        mAuth.signInWithEmailAndPassword(email, password)
                .addOnCompleteListener(this, new OnCompleteListener<AuthResult>() {
                    @Override
                    public void onComplete(@NonNull Task<AuthResult> task) {
                        if (task.isSuccessful()) {
                            Toast.makeText(MainActivity.this, "login ok", Toast.LENGTH_LONG).show();
                            callback.onLoginSuccess();
                        } else {
                            Toast.makeText(MainActivity.this, "login fail", Toast.LENGTH_LONG).show();
                            callback.onLoginFail();
                        }
                    }
                });
    }


    public void addDATA()
    {

        String phone = ((EditText) findViewById(R.id.editTextPhone)).getText().toString();
        String email = ((EditText) findViewById(R.id.editTextEmailAddressAtRegisterPage)).getText().toString();

        FirebaseDatabase database = FirebaseDatabase.getInstance();
        DatabaseReference myRef = database.getReference("users").child(phone);

        Student s = new Student(email, phone);

        myRef.setValue(s);
    }

    public void getStudent(String phone){

        FirebaseDatabase database = FirebaseDatabase.getInstance();
        DatabaseReference myRef = database.getReference("users").child(phone);

        myRef.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(DataSnapshot dataSnapshot) {
                // This method is called once with the initial value and again
                // whenever data at this location is updated.
                Student value = dataSnapshot.getValue(Student.class);
                //Toast.makeText(MainActivity.this , value.getEmail(), Toast.LENGTH_LONG).show();
            }

            @Override
            public void onCancelled(DatabaseError error) {
                // Failed to read value

            }
        });
    }
}