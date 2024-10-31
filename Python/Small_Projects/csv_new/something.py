import pandas as pd

df = pd.read_csv('C:\python_projects_ohad\py_one_hundred_days_of_code_projects\csv_new\salaries_by_college_major.csv')
clean_df = df.dropna()


spread_col = clean_df['Mid-Career 90th Percentile Salary'].subtract(clean_df['Mid-Career 10th Percentile Salary'])
clean_df.insert(1, 'Spread', spread_col)

low_risk = clean_df.sort_values('Spread')
print(clean_df.groupby('Group').mean())