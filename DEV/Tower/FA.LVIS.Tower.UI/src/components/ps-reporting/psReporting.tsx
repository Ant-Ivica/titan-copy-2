 
import React, { useState, useEffect } from 'react';
import Modal from './Modal'; // Assuming Modal component handles modal logic similar to AngularJS modalProvider
import ReportingComponent from './reporting-row-detail'; // Import the ReportingComponent from reporting-row-detail.tsx

const PsReporting = () => {
    const [data, setData] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [selectedItem, setSelectedItem] = useState(null); // State to handle selected item for modal
    const [selectedRowKeys, setSelectedRowKeys] = useState([]); // State to handle row selection

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

    const handleRowSelectionChange = (selectedKeys, selectedRows) => {
        setSelectedRowKeys(selectedKeys);
        // Additional logic can be added here if needed
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
                    activeCustomerName="Active Customer Name" // This should be dynamic based on context
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
