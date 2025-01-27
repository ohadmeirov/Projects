package com.example.MyTrip.fragments.continentsFragments;

import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;

import com.example.MyTrip.R;
import com.example.MyTrip.fragments.countriesFragments.CountriesAdapter;

import java.util.ArrayList;
import java.util.List;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link EuropeFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class EuropeFragment extends Fragment {

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    private RecyclerView recyclerViewCountries;

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    public EuropeFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment EuropeFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static EuropeFragment newInstance(String param1, String param2) {
        EuropeFragment fragment = new EuropeFragment();
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
        // Inflate the fragment layout
        View view = inflater.inflate(R.layout.fragment_europe, container, false);

        // Initialize RecyclerView
        recyclerViewCountries = view.findViewById(R.id.recyclerViewEurope);
        recyclerViewCountries.setLayoutManager(new LinearLayoutManager(getContext()));

        // Set the adapter
        CountriesAdapter adapter = new CountriesAdapter(getEuropeCountries());
        recyclerViewCountries.setAdapter(adapter);
        // Initialize SearchBox
        EditText searchBox = view.findViewById(R.id.editTextSearchEurope);
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

    private List<String> getEuropeCountries() {
        List<String> countries = new ArrayList<>();
        countries.add("Germany");
        countries.add("France");
        countries.add("Italy");
        countries.add("Spain");
        countries.add("United Kingdom");
        countries.add("Czech Republic");
        countries.add("Sweden");
        countries.add("Ukraine");
        countries.add("Belgium");
        countries.add("Netherlands");
        countries.add("Switzerland");
        countries.add("Greece");
        countries.add("Portugal");
        countries.add("Hungary");
        countries.add("Israel");
        return countries;
    }
}
