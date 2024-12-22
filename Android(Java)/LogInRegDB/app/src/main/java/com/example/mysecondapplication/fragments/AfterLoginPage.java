package com.example.mysecondapplication.fragments;

import android.os.Bundle;

import androidx.fragment.app.Fragment;
import android.content.Context;
import android.content.SharedPreferences;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.example.mysecondapplication.R;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link AfterLoginPage#newInstance} factory method to
 * create an instance of this fragment.
 */
public class AfterLoginPage extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    public AfterLoginPage() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment FragmentThree.
     */
    // TODO: Rename and change types and number of parameters
    public static AfterLoginPage newInstance(String param1, String param2) {
        AfterLoginPage fragment = new AfterLoginPage();
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
        View view = inflater.inflate(R.layout.after_login_page, container, false);

        SharedPreferences sharedPreferences = getActivity().getSharedPreferences("UserPrefs", Context.MODE_PRIVATE);
        String userName = sharedPreferences.getString("userName", "Can't find UserName");


        TextView textView = view.findViewById(R.id.textViewOfUserName);
        textView.setText("Hello, " + userName + "!");

        Button buttonPlus = view.findViewById(R.id.buttonPlus);
        Button buttonMinus = view.findViewById(R.id.buttonMinus);
        TextView textViewCounter = view.findViewById(R.id.textViewCounter);

        final int[] counter = {0};

        buttonPlus.setOnClickListener(v -> {
            counter[0]++;
            textViewCounter.setText(String.valueOf(counter[0]));
        });

        buttonMinus.setOnClickListener(v -> {
            if (counter[0] > 0){
                counter[0]--;
                textViewCounter.setText(String.valueOf(counter[0]));
            }
        });


        return view;
    }
}