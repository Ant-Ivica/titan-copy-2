 
import React, { useState, useEffect } from 'react';
import ReportingComponent from './reporting-row-detail'; // Import the ReportingComponent
import Modal from './Modal'; // Assuming Modal component handles modal logic similar to AngularJS modalProvider

const PsReporting = () => {
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

    const handleRowSelectionChange = (selectedRowKeys, selectedRows) => {
        setSelectedItem(selectedRows[0]); // Assuming we want to handle the first selected item
    };

    return (
        <div>
            {isLoading ? (
                <p>Loading...</p>
            ) : error ? (
                <p>Error: {error}</p>
            ) : (
                <ReportingComponent
                    rows={data}
                    activeCustomerName={selectedItem ? selectedItem.CustomerId : ''}
                    onRowSelectionChange={handleRowSelectionChange}
                />
            )}
            {selectedItem && (
                <Modal item={selectedItem} onClose={() => setSelectedItem(null)} />
            )}
        </div>
    );
};

export default PsReporting;
