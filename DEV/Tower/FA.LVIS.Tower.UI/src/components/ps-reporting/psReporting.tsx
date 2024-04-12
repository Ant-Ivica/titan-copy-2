 
import React, { useState, useEffect } from 'react';
import Modal from './Modal'; // Importing a generic Modal component for handling popups

const ReportingComponent = () => {
    const [data, setData] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [selectedRow, setSelectedRow] = useState(null); // State to handle selected row data

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

    const handleRowClick = (row) => {
        setSelectedRow(row);
        // Assuming Modal component accepts an 'isOpen' prop to show/hide and 'content' prop for modal content
        Modal.open({
            content: <div>{row.detail}</div> // Displaying row details in modal
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
                        <li key={index} onClick={() => handleRowClick(item)}>{item.name}</li> // Adjust according to data structure and add onClick
                    ))}
                </ul>
            )}
            {selectedRow && <Modal />} // Render Modal based on selectedRow state
        </div>
    );
};

export default ReportingComponent;
