package com.example.MyTrip.fragments.startLogRegFragments;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.example.MyTrip.R;
import com.example.MyTrip.fragments.continentsFragments.ContinentsAdapter;

public class AfterLoginPage extends Fragment {

    private RecyclerView recyclerViewContinents;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the fragment layout
        View view = inflater.inflate(R.layout.after_login_page, container, false);

        TextView userNameTextView = view.findViewById(R.id.textViewOfUserName);
        SharedPreferences sharedPreferences = getActivity().getSharedPreferences("UserPrefs", Context.MODE_PRIVATE);
        String userName = sharedPreferences.getString("userName", "");
        userNameTextView.setText("Hello, " + userName + "!");


        // Initialize RecyclerView
        recyclerViewContinents = view.findViewById(R.id.recyclerViewContinents);
        recyclerViewContinents.setLayoutManager(new LinearLayoutManager(getContext()));

        // Set the adapter
        ContinentsAdapter adapter = new ContinentsAdapter();
        recyclerViewContinents.setAdapter(adapter);

        return view;
    }
}

