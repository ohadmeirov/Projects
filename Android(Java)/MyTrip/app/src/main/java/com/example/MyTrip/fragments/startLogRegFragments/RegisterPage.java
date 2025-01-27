package com.example.MyTrip.fragments.startLogRegFragments;

import android.os.Bundle;
import android.content.Context;
import android.content.SharedPreferences;

import androidx.fragment.app.Fragment;
import androidx.navigation.Navigation;
import com.example.MyTrip.interfaces.RegisterCallback;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.example.MyTrip.R;
import com.example.MyTrip.activities.MainActivity;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link RegisterPage#newInstance} factory method to
 * create an instance of this fragment.
 */
public class RegisterPage extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    public RegisterPage() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment FragmentTwo.
     */
    // TODO: Rename and change types and number of parameters
    public static RegisterPage newInstance(String param1, String param2) {
        RegisterPage fragment = new RegisterPage();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.register_page, container, false);




        Button button1 = view.findViewById(R.id.RegisterButtonFromRegisterPageToMainPage);
        Button backButton = view.findViewById(R.id.BackFromRegisterPage);
        EditText emailEditText = view.findViewById(R.id.editTextEmailAddressAtRegisterPage);
        EditText passwordEditText = view.findViewById(R.id.editTextPasswordAtRegisterPage);
        EditText verifyPasswordEditText = view.findViewById(R.id.editTextRePasswordAtRegisterPage);
        EditText nameEditText = view.findViewById(R.id.editTextUserName);
        EditText phoneEditText = view.findViewById(R.id.editTextPhone);

        button1.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String email = emailEditText.getText().toString();
                String password = passwordEditText.getText().toString();
                String verifyPassword = verifyPasswordEditText.getText().toString();
                String name = nameEditText.getText().toString();
                String phone = phoneEditText.getText().toString();

                if (email.length() > 256 || password.length() > 256 || verifyPassword.length() > 256 ||
                        name.length() > 256 || phone.length() > 256) {
                    Toast.makeText(getContext(), "Input fields cannot exceed 256 characters!", Toast.LENGTH_LONG).show();
                    return;
                }

                if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
                    Toast.makeText(getContext(), "Invalid email format!", Toast.LENGTH_LONG).show();
                    return;
                }

                if (password.length() < 6) {
                    Toast.makeText(getContext(), "Password must be at least 6 characters long!", Toast.LENGTH_LONG).show();
                    return;
                }

                if (!password.equals(verifyPassword)) {
                    Toast.makeText(getContext(), "The two passwords do not match!", Toast.LENGTH_LONG).show();
                    return;
                }

                String userName = nameEditText.getText().toString();
                SharedPreferences sharedPreferences = getActivity().getSharedPreferences("UserPrefs", Context.MODE_PRIVATE);
                SharedPreferences.Editor editor = sharedPreferences.edit();
                editor.putString("userName", userName);
                editor.apply();

                MainActivity mainActivity = (MainActivity) getActivity();
                mainActivity.register(new RegisterCallback() {
                    @Override
                    public void onRegisterSuccess() {
                        Navigation.findNavController(view).navigate(R.id.action_registerPage_to_mainPage);
                    }

                    @Override
                    public void onRegisterFail() {
                        Toast.makeText(getContext(), "Registration failed. Please try again!", Toast.LENGTH_LONG).show();
                    }
                });

                mainActivity.addDATA();
            }
        });


        backButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                        Navigation.findNavController(view).navigate(R.id.action_registerPage_to_mainPage);
                    }
        });
        return view;
    }
}
