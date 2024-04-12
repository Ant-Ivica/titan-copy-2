 
import React, { useState, useEffect } from 'react';
import { Table, Button } from 'antd';
import ReportingComponent from './reporting-row-detail'; // Import the ReportingComponent

interface RowData {
    ServiceRequestId: string;
    createddate: string;
    service: string;
    ApplicationId: string;
    CustomerId: string;
    ExternalRefNum: string;
    CustomerRefNum: string;
    InternalRefNum?: string;
    InternalRefId?: string;
}

interface ReportingProps {
    rows: RowData[];
    activeCustomerName: string;
    onRowSelectionChange: (selectedRowKeys: React.Key[], selectedRows: RowData[]) => void;
}

const PsReporting: React.FC<ReportingProps> = ({ rows, activeCustomerName, onRowSelectionChange }) => {
    // State and handlers can be managed here if needed for additional functionality

    return (
        <div>
            {/* Utilize the imported ReportingComponent */}
            <ReportingComponent
                rows={rows}
                activeCustomerName={activeCustomerName}
                onRowSelectionChange={onRowSelectionChange}
            />
        </div>
    );
};

export default PsReporting;
