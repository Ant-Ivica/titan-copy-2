 
import React, { useEffect } from 'react'; 
import Modal from 'react-modal'; 
import { toast, ToastContainer } from 'react-toastify'; 
import 'react-toastify/dist/ReactToastify.css'; 
import { confirmAlert } from 'react-confirm-alert'; 
import 'react-confirm-alert/src/react-confirm-alert.css'; 
import Cookies from 'universal-cookie'; 
import psReportingService from '../../services/ps-reporting/psReporting.service'; 
import { useDispatch, useSelector } from 'react-redux'; 
import { 
    setOrderToInvalidate, 
    setInvalidBtnEnable, 
    setLoggedTenant, 
    setTogglingTenant, 
    setHasAccess, 
    setHasSuperAccess, 
    setFromDate, 
    setThroughDate, 
    setBusy, 
    setFilterSection, 
    setDisableDate, 
    setData 
} from './modules/psReportingActions'; 

const ReportingComponent = () => { 
    const dispatch = useDispatch(); 
    const { 
        data, 
        isLoading, 
        error, 
        orderToInvalidate, 
        inValidBtnEnable, 
        loggedTenant, 
        togglingTenant, 
        hasAccess, 
        hasSuperAccess, 
        fromDate, 
        throughDate, 
        busy, 
        filterSection, 
        disableDate 
    } = useSelector(state => state.reporting); 
    const cookies = new Cookies(); 

    useEffect(() => { 
        dispatch(setBusy(true)); 
        const fetchData = async () => { 
            try { 
                const response = await psReportingService.getTenant(); 
                dispatch(setLoggedTenant(response.data)); 
                dispatch(setTogglingTenant(response.data)); 
            } catch (error) { 
                toast.error(error.message); 
            } 
            dispatch(setBusy(false)); 
        }; 
        fetchData(); 
    }, [dispatch]); 

    const invalidateOrders = async () => { 
        try { 
            const response = await psReportingService.invalidateOrderData(orderToInvalidate); 
            dispatch(setOrderToInvalidate([])); 
            toast.success('Orders invalidated successfully'); 
            fetchData(); 
        } catch (error) { 
            toast.error('Failed to invalidate orders'); 
        } 
    }; 

    const fetchData = async () => { 
        dispatch(setBusy(true)); 
        try { 
            const response = filterSection === '1' ? 
                await psReportingService.getReportDetails(togglingTenant, { Fromdate: fromDate, ThroughDate: throughDate }) : 
                await psReportingService.getReportDetailsFilter(filterSection, togglingTenant); 
            dispatch(setData(response.data)); 
        } catch (error) { 
            toast.error(error.message); 
        } 
        dispatch(setBusy(false)); 
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
