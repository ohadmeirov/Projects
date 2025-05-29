import React, { useEffect, useRef } from 'react';
import * as d3 from 'd3';
import axios from 'axios';
import { Paper, Typography, Box } from '@mui/material';

const Stats = () => {
    const postsPerMonthRef = useRef();
    const usersPerMonthRef = useRef();  // שינוי שם הרפרנס

    useEffect(() => {
        // גרף 1: פוסטים חדשים בכל חודש
        axios.get('http://localhost:5000/api/stats/posts-per-month')
            .then(res => {
                const data = res.data;
                const svg = d3.select(postsPerMonthRef.current);
                svg.selectAll('*').remove();
                const width = 400, height = 200;
                svg.attr('width', width).attr('height', height);

                const x = d3.scaleBand()
                    .domain(data.map(d => d.month))
                    .range([40, width - 10])
                    .padding(0.1);

                const y = d3.scaleLinear()
                    .domain([0, d3.max(data, d => d.count)])
                    .nice()
                    .range([height - 30, 10]);

                // תיקון הצגת הצירים - רק מספרים שלמים
                const yAxis = d3.axisLeft(y)
                    .tickFormat(d3.format('d'))  // פורמט למספרים שלמים
                    .ticks(Math.min(10, d3.max(data, d => d.count)))  // מקסימום 10 תגיות או פחות לפי הערך המקסימלי
                    .tickValues(d3.range(0, d3.max(data, d => d.count) + 1, 1));  // ערכים שלמים בלבד

                // תיקון הצגת הצירים
                const xAxis = d3.axisBottom(x);
                svg.append('g')
                    .attr('transform', `translate(0,${height - 30})`)
                    .call(xAxis)
                    .selectAll('text')  // הוסף את זה אחרי ה-call
                    .style('text-anchor', 'end')
                    .attr('dx', '-.8em')
                    .attr('dy', '.15em')
                    .attr('transform', 'rotate(-45)');

                    // הוספת כותרות צירים
                svg.append('text')
                    .attr('x', width / 2 - 40)
                    .attr('y', height - 10)
                    .style('text-anchor', 'middle')
                    .text('Month');

                svg.append('g')
                    .attr('transform', `translate(40,0)`)
                    .call(yAxis);

                svg.selectAll('rect')
                    .data(data)
                    .enter()
                    .append('rect')
                    .attr('x', d => x(d.month))
                    .attr('y', d => y(d.count))
                    .attr('width', x.bandwidth())
                    .attr('height', d => height - 30 - y(d.count))
                    .attr('fill', '#2196F3');
            });

        // גרף 2: משתמשים חדשים בכל חודש
        axios.get('http://localhost:5000/api/stats/users-per-month')
            .then(res => {
                const data = res.data;
                const svg = d3.select(usersPerMonthRef.current);
                svg.selectAll('*').remove();
                const width = 400, height = 200;
                svg.attr('width', width).attr('height', height);

                const x = d3.scaleBand()
                    .domain(data.map(d => d.month))
                    .range([40, width - 10])
                    .padding(0.1);

                const y = d3.scaleLinear()
                    .domain([0, d3.max(data, d => d.count)])
                    .nice()
                    .range([height - 30, 10]);

                // תיקון הצגת הצירים - רק מספרים שלמים
                const yAxis = d3.axisLeft(y)
                    .tickFormat(d3.format('d'))  // פורמט למספרים שלמים
                    .ticks(Math.min(10, d3.max(data, d => d.count)))  // מקסימום 10 תגיות או פחות לפי הערך המקסימלי
                    .tickValues(d3.range(0, d3.max(data, d => d.count) + 1, 1));  // ערכים שלמים בלבד

                // תיקון הצגת הצירים
                const xAxis = d3.axisBottom(x);
                svg.append('g')
                    .attr('transform', `translate(0,${height - 30})`)
                    .call(xAxis)
                    .selectAll('text')  // הוסף את זה אחרי ה-call
                    .style('text-anchor', 'end')
                    .attr('dx', '-.8em')
                    .attr('dy', '.15em')
                    .attr('transform', 'rotate(-45)');

                svg.append('g')
                    .attr('transform', `translate(40,0)`)
                    .call(yAxis);

                // הוספת כותרות צירים
                svg.append('text')
                    .attr('x', width / 2 - 40)
                    .attr('y', height - 10)
                    .style('text-anchor', 'middle')
                    .text('Month');

                svg.selectAll('rect')
                    .data(data)
                    .enter()
                    .append('rect')
                    .attr('x', d => x(d.month))
                    .attr('y', d => y(d.count))
                    .attr('width', x.bandwidth())
                    .attr('height', d => height - 30 - y(d.count))
                    .attr('fill', '#FF4081');
            });
    }, []);

    return (
        <Box sx={{ p: 3 }}>
            <Paper elevation={3} sx={{
                p: 3,
                mb: 3,
                minHeight: '400px',  // הוספת גובה מינימלי
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center'
            }}>
                <Typography variant="h5" gutterBottom>Posts Per Month</Typography>
                <svg ref={postsPerMonthRef} style={{ width: '100%', height: '300px' }}></svg>
            </Paper>
            <Paper elevation={3} sx={{
                p: 3,
                minHeight: '400px',  // הוספת גובה מינימלי
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center'
            }}>
                <Typography variant="h5" gutterBottom>New Users Per Month</Typography>
                <svg ref={usersPerMonthRef} style={{ width: '100%', height: '300px' }}></svg>
            </Paper>
        </Box>
    );
};

export default Stats;
