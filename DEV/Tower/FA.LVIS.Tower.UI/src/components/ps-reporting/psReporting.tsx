 
import React, { useState, useEffect } from 'react';
import ReportingRowDetail from './ReportingRowDetail'; // Assuming the converted TSX file is named ReportingRowDetail

const ReportingComponent = () => {
    const [data, setData] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [selectedRow, setSelectedRow] = useState(null);

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
                        <li key={index} onClick={() => handleRowClick(item)}>
                            {item.name} // Adjust according to data structure
                        </li>
                    ))}
                </ul>
            )}
            {selectedRow && (
                <ReportingRowDetail row={selectedRow} />
            )}
        </div>
    );
};

export default ReportingComponent;
