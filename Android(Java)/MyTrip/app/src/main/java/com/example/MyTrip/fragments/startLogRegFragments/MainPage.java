package com.example.MyTrip.fragments.startLogRegFragments;

import android.os.Bundle;
import com.example.MyTrip.interfaces.LoginCallback;

import androidx.fragment.app.Fragment;
import androidx.navigation.Navigation;

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
 * Use the {@link MainPage#newInstance} factory method to
 * create an instance of this fragment.
 */
public class MainPage extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;


    public MainPage() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment FragmentOne.
     */
    // TODO: Rename and change types and number of parameters
    public static MainPage newInstance(String param1, String param2) {
        MainPage fragment = new MainPage();
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
        View view = inflater.inflate(R.layout.main_page, container, false);
        EditText emailEditText = view.findViewById(R.id.editTextEmailAddressAtMainPage);
        EditText passwordEditText = view.findViewById(R.id.editTextPasswordAtMainPage);

        Button button1 = view.findViewById(R.id.LoginButtonFromMainPageToAfterLoginPage);

        button1.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                MainActivity mainActivity = (MainActivity) getActivity();
                if (mainActivity == null) {
                    return;
                }

                String email = emailEditText.getText().toString();
                String password = passwordEditText.getText().toString();

                if (email.isEmpty() || password.isEmpty()) {
                    Toast.makeText(getActivity(), "Email and Password cannot be empty!", Toast.LENGTH_LONG).show();
                    return;
                }

                mainActivity.login(new LoginCallback() {
                    @Override
                    public void onLoginSuccess() {
                        if (getView() != null) {
                            Navigation.findNavController(getView()).navigate(R.id.action_mainPage_to_AfterLoginPage);
                        }
                    }

                    @Override
                    public void onLoginFail() {}
                });
            }
        });

        Button button2 = view.findViewById(R.id.RegisterButtonFromMainPageToRegisterPage);

        button2.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (getView() != null) {
                    Navigation.findNavController(getView()).navigate(R.id.action_mainPage_to_registerPage);
                }
            }
        });

        return view;
    }
}