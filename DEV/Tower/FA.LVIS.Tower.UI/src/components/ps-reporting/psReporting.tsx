import React, { useState, useEffect } from 'react';

const ReportingComponent = () => {
    const [data, setData] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            setIsLoading(true);
            try {
                const response = await fetch('/api/data'); // Adjust API endpoint as needed
                if (!response.ok) throw new Error('Network response was not ok');
                const result = await response.json();
                setData(result);
            } catch (error) {
                setError(error.message);
            }
            setIsLoading(false);
        };

        fetchData();
    }, []);

    return (
        <div>
            {isLoading ? (
                <p>Loading...</p>
            ) : error ? (
                <p>Error: {error}</p>
            ) : (
                <ul>
                    {data.map((item, index) => (
                        <li key={index}>{item.name}</li> // Adjust according to data structure
                    ))}
                </ul>
            )}
        </div>
    );
};

export default ReportingComponent;