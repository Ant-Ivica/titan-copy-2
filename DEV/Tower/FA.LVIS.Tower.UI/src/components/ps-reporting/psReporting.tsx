 
import React, { useState, useEffect } from 'react';
import Modal from './Modal'; // Assuming Modal component handles modal logic similar to AngularJS modalProvider

const ReportingComponent = () => {
    const [data, setData] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [selectedItem, setSelectedItem] = useState(null); // State to handle selected item for modal

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

    const handleRowDoubleClick = (item) => {
        setSelectedItem(item);
        // Open modal with selected item details
        // This simulates the editReportRow functionality from AngularJS
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
            {selectedItem && (
                <Modal item={selectedItem} onClose={() => setSelectedItem(null)} />
            )}
        </div>
    );
};

export default ReportingComponent;
