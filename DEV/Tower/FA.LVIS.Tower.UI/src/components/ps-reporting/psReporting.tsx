import React, { useEffect, useMemo, useState } from 'react'; 
import { useTable, useExpanded } from 'react-table'; 
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
    setDisableDate 
} from './modules/psReportingActions'; 
import PsReportingComponent from './ps-reporting/psReportingTemplate'; // Import the PsReportingComponent

const ReportingComponent = () => { 
    const data = useSelector(state => state.data); 
    const isLoading = useSelector(state => state.isLoading); 
    const error = useSelector(state => state.error); 
    const dispatch = useDispatch(); 
    const cookies = new Cookies(); 

    useEffect(() => { 
        fetchData(); 
    }, [dispatch]); 

    const columns = useMemo(() => [
        { 
            Header: 'Name', 
            accessor: 'name' 
        }, 
        { 
            Header: 'Details', 
            accessor: 'details', 
            Cell: ({ row }) => (
                <div>
                    {row.isExpanded ? (
                        <div>
                            {/* Convert reporting-row-detail.html content to JSX here */}
                            <p>Details for {row.original.name}</p>
                        </div>
                    ) : null}
                </div>
            )
        }
    ], []);

    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        rows,
        prepareRow,
        visibleColumns,
        state: { expanded },
    } = useTable({ columns, data }, useExpanded);

    return ( 
        <div> 
            <ToastContainer /> 
            {isLoading ? ( 
                <p>Loading...</p> 
            ) : error ? ( 
                <p>Error: {error}</p> 
            ) : ( 
                <table {...getTableProps()}>
                    <thead>
                        {headerGroups.map(headerGroup => (
                            <tr {...headerGroup.getHeaderGroupProps()}>
                                {headerGroup.headers.map(column => (
                                    <th {...column.getHeaderProps()}>{column.render('Header')}</th>
                                ))}
                            </tr>
                        ))}
                    </thead>
                    <tbody {...getTableBodyProps()}>
                        {rows.map(row => {
                            prepareRow(row);
                            return (
                                <tr {...row.getRowProps()}>
                                    {row.cells.map(cell => {
                                        return <td {...cell.getCellProps()}>{cell.render('Cell')}</td>;
                                    })}
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            )} 
            <PsReportingComponent 
                loggedTenant={cookies.get('loggedTenant')}
                togglingTenant={cookies.get('togglingTenant')}
                hasAccess={cookies.get('hasAccess')}
                search={psReportingService.searchOrders}
                searchbyReferenceNo={psReportingService.searchByReference}
                loadRFOrder={psReportingService.loadRFOrders}
                inValidateConfirm={confirmAlert}
                title="Reporting Dashboard"
            />
        </div> 
    ); 
}; 

export default ReportingComponent; 
