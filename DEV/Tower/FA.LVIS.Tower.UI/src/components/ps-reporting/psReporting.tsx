import React, { useState, useEffect } from 'react';
import Modal from 'react-modal';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { confirmAlert } from 'react-confirm-alert';
import 'react-confirm-alert/src/react-confirm-alert.css';
import Cookies from 'universal-cookie';

const ReportingComponent = () => {
    const [data, setData] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const cookies = new Cookies();

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
                toast.error(error.message);
            }
            setIsLoading(false);
        };

        fetchData();
    }, []); // Dependency array left empty to mimic componentDidMount behavior

    const handleConfirm = () => {
        confirmAlert({
            title: 'Confirm to submit',
            message: 'Are you sure to do this?',
            buttons: [
                {
                    label: 'Yes',
                    onClick: () => toast.success('Clicked Yes')
                },
                {
                    label: 'No',
                    onClick: () => toast.info('Clicked No')
                }
            ]
        });
    };

    return (
        <div>
            <ToastContainer />
            {isLoading ? (
                <p>Loading...</p>
            ) : error ? (
                <p>Error: {error}</p>
            ) : (
                <ul>
                    {data.map(item => (
                        <li key={item.id}>{item.name}</li> // Adjust according to actual data structure
                    ))}
                </ul>
            )}
            <button onClick={handleConfirm}>Confirm Action</button>
        </div>
    );
};

export default ReportingComponent;