 
import React, { useState, useEffect } from 'react';
import Modal from './Modal'; // Assuming Modal component exists for handling modals similar to AngularJS modalProvider

const ReportingComponent = () => {
    const [data, setData] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [selectedRow, setSelectedRow] = useState(null); // State to handle selected row

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

    const handleRowDoubleClick = (row) => {
        setSelectedRow(row);
        // Simulate opening a modal with row details, similar to AngularJS modalProvider
        Modal.open({
            content: <div>{row.details}</div>, // Assuming row.details contains the necessary details
            onClose: () => setSelectedRow(null)
        });
    };

    return (
        <div>
            {isLoading ? (
                <p>Loading...</p>
            ) : error ? (
                <p>Error: {error}</p>
            ) : (
                <ul>
                    {data.map((item, index) => (
                        <li key={index} onDoubleClick={() => handleRowDoubleClick(item)}>
                            {item.name} // Adjust according to data structure
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default ReportingComponent;
