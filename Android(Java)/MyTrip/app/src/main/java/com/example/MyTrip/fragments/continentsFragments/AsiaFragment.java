package com.example.MyTrip.fragments.continentsFragments;


import android.os.Bundle;
import com.example.MyTrip.R;
import com.example.MyTrip.fragments.countriesFragments.CountriesAdapter;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;

import java.util.ArrayList;
import java.util.List;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link AsiaFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class AsiaFragment extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    private RecyclerView recyclerViewCountries;

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;
    private CountriesAdapter adapter;

    private EditText searchBox;

    public AsiaFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment AsiaFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static AsiaFragment newInstance(String param1, String param2) {
        AsiaFragment fragment = new AsiaFragment();
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
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        // Inflate the fragment layout
        View view = inflater.inflate(R.layout.fragment_asia, container, false);

        // Initialize RecyclerView
        RecyclerView recyclerViewCountries = view.findViewById(R.id.recyclerViewAsia);
        recyclerViewCountries.setLayoutManager(new LinearLayoutManager(getContext()));

        // Set the adapter (define it as a class member)
        adapter = new CountriesAdapter(getAsiaCountries());
        recyclerViewCountries.setAdapter(adapter);

        // Initialize SearchBox
        EditText searchBox = view.findViewById(R.id.editTextSearchAsia);
        searchBox.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {
            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                // Filter adapter
                if (adapter != null) {
                    adapter.getFilter().filter(s);
                }
            }

            @Override
            public void afterTextChanged(Editable s) {
            }
        });

        return view;
    }

    private List<String> getAsiaCountries() {
        List<String> countries = new ArrayList<>();
        countries.add("China");
        countries.add("India");
        countries.add("Japan");
        countries.add("South Korea");
        countries.add("Indonesia");
        countries.add("Pakistan");
        countries.add("Bangladesh");
        countries.add("Russia");
        countries.add("Saudi Arabia");
        countries.add("Thailand");
        countries.add("Vietnam");
        countries.add("Malaysia");
        countries.add("Nepal");
        return countries;
    }
}
