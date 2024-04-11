 
import React, { useEffect, useState } from 'react'; 
import Modal from 'react-modal'; 
import { toast, ToastContainer } from 'react-toastify'; 
import 'react-toastify/dist/ReactToastify.css'; 
import { confirmAlert } from 'react-confirm-alert'; 
import 'react-confirm-alert/src/react-confirm-alert.css'; 
import Cookies from 'universal-cookie'; 
import psReportingService from '../../services/psReporting.service'; 
import { useSelector, useDispatch } from 'react-redux'; 
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
    setData, 
    setError 
} from './modules/psReportingActions'; 
import ReportingRowDetail from './ReportingRowDetail'; 
import PsReportingComponent from './ps-reporting/psReportingTemplate'; 
const ReportingComponent = () => { 
    const data = useSelector(state => state.data); 
    const isLoading = useSelector(state => state.isLoading); 
    const error = useSelector(state => state.error); 
    const orderToInvalidate = useSelector(state => state.orderToInvalidate); 
    const inValidBtnEnable = useSelector(state => state.inValidBtnEnable); 
    const loggedTenant = useSelector(state => state.loggedTenant); 
    const togglingTenant = useSelector(state => state.togglingTenant); 
    const hasAccess = useSelector(state => state.hasAccess); 
    const hasSuperAccess = useSelector(state => state.hasSuperAccess); 
    const fromDate = useSelector(state => state.fromDate); 
    const throughDate = useSelector(state => state.throughDate); 
    const busy = useSelector(state => state.busy); 
    const filterSection = useSelector(state => state.filterSection); 
    const disableDate = useSelector(state => state.disableDate); 
    const dispatch = useDispatch(); 
    const cookies = new Cookies(); 
    useEffect(() => { 
        const fetchData = async () => { 
            dispatch(setBusy(true)); 
            try { 
                const response = await psReportingService.getTenant(); 
                dispatch(setLoggedTenant(response.data)); 
                dispatch(setTogglingTenant(response.data)); 
            } catch (error) { 
                dispatch(setError(error.message)); 
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
            dispatch(setError(error.message)); 
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
                        <li key={item.id}>{item.name}</li> 
                    ))} 
                </ul> 
            )} 
            <button onClick={handleConfirm}>Confirm Action</button> 
            <PsReportingComponent 
                loggedTenant={loggedTenant} 
                togglingTenant={togglingTenant} 
                hasAccess={hasAccess} 
                search={fetchData} 
                searchbyReferenceNo={fetchData} 
                loadRFOrder={fetchData} 
                inValidateConfirm={handleConfirm} 
                title="Reporting Dashboard" 
            /> 
        </div> 
    ); 
}; 
export default ReportingComponent; 
