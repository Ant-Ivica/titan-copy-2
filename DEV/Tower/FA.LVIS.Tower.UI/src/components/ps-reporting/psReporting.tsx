 
import React, { useState, useEffect } from 'react';
import ReportingComponent from './reporting-row-detail'; // Import the ReportingRowDetail component

const PsReporting = () => {
    const [data, setData] = useState([]);
    const [selectedRowKeys, setSelectedRowKeys] = useState([]);
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

    const handleRowSelectionChange = (selectedRowKeys, selectedRows) => {
        setSelectedRowKeys(selectedRowKeys);
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
                    activeCustomerName="SpecificCustomerName" // Adjust as needed
                    onRowSelectionChange={handleRowSelectionChange}
                />
            )}
        </div>
    );
};

export default PsReporting;
