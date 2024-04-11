 
import React, { useState, useEffect } from 'react';
import Modal from 'react-modal';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { confirmAlert } from 'react-confirm-alert';
import 'react-confirm-alert/src/react-confirm-alert.css';
import Cookies from 'universal-cookie';
import psReportingService from '../../services/psReporting.service';

const ReportingComponent = () => {
    const [data, setData] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [orderToInvalidate, setOrderToInvalidate] = useState([]);
    const [inValidBtnEnable, setInValidBtnEnable] = useState(true);
    const [loggedTenant, setLoggedTenant] = useState('');
    const [togglingTenant, setTogglingTenant] = useState('');
    const [hasAccess, setHasAccess] = useState(false);
    const [hasSuperAccess, setHasSuperAccess] = useState(false);
    const [fromDate, setFromDate] = useState('');
    const [throughDate, setThroughDate] = useState('');
    const [busy, setBusy] = useState(false);
    const [filterSection, setFilterSection] = useState('7');
    const [disableDate, setDisableDate] = useState(true);
    const cookies = new Cookies();

    useEffect(() => {
        const fetchData = async () => {
            setIsLoading(true);
            try {
                const response = await psReportingService.getTenant();
                setLoggedTenant(response.data);
                setTogglingTenant(response.data);
            } catch (error) {
                setError(error.message);
                toast.error(error.message);
            }
            setIsLoading(false);
        };

        fetchData();
    }, []);

    const invalidateOrders = async () => {
        try {
            const response = await psReportingService.invalidateOrderData(orderToInvalidate);
            setOrderToInvalidate([]);
            toast.success('Orders invalidated successfully');
            fetchData();
        } catch (error) {
            toast.error('Failed to invalidate orders');
        }
    };

    const fetchData = async () => {
        setIsLoading(true);
        try {
            const response = filterSection === '1' ? 
                await psReportingService.getReportDetails(togglingTenant, { Fromdate: fromDate, ThroughDate: throughDate }) : 
                await psReportingService.getReportDetailsFilter(filterSection, togglingTenant);
            setData(response.data);
        } catch (error) {
            setError(error.message);
            toast.error(error.message);
        }
        setIsLoading(false);
    };

    const handleConfirm = () => {
        confirmAlert({
            title: 'Confirm to submit',
            message: 'Are you sure to do this?',
            buttons: [
                {
                    label: 'Yes',
                    onClick: () => invalidateOrders()
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
