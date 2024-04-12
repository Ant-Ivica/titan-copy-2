import React, { useState } from 'react';
import { Table } from 'antd';
import ReportingTemplate from './ps-reporting/psReportingTemplate'; // Import the ReportingTemplate component

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

const ReportingComponent: React.FC<ReportingProps> = ({ rows, activeCustomerName, onRowSelectionChange }) => {
    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);

    const handleSelectChange = (selectedRowKeys: React.Key[], selectedRows: RowData[]) => {
        setSelectedRowKeys(selectedRowKeys);
        onRowSelectionChange(selectedRowKeys, selectedRows);
    };

    const columns = [
        {
            title: 'Service Request Id',
            dataIndex: 'ServiceRequestId',
            key: 'ServiceRequestId',
        },
        {
            title: 'Order Date',
            dataIndex: 'createddate',
            key: 'createddate',
            render: (text: string) => new Date(text).toLocaleDateString('en-US'),
        },
        {
            title: 'Service Type',
            dataIndex: 'service',
            key: 'service',
        },
        {
            title: 'Application',
            dataIndex: 'ApplicationId',
            key: 'ApplicationId',
        },
        {
            title: 'Customer Id',
            dataIndex: 'CustomerId',
            key: 'CustomerId',
        },
        {
            title: 'External Reference Number',
            dataIndex: 'ExternalRefNum',
            key: 'ExternalRefNum',
        },
        {
            title: 'Customer Reference Number',
            dataIndex: 'CustomerRefNum',
            key: 'CustomerRefNum',
        },
        {
            title: 'Internal Reference Number',
            dataIndex: 'InternalRefNum',
            key: 'InternalRefNum',
        },
        {
            title: 'Internal Reference Id',
            dataIndex: 'InternalRefId',
            key: 'InternalRefId',
        },
    ];

    const rowSelection = {
        selectedRowKeys,
        onChange: handleSelectChange,
    };

    return (
        <div>
            <ReportingTemplate /> {/* Render the imported ReportingTemplate component */}
            <Table
                rowSelection={rowSelection}
                columns={columns}
                dataSource={rows.filter(row => row.InternalRefNum === activeCustomerName)}
                rowKey="ServiceRequestId"
            />
        </div>
    );
};

export default ReportingComponent;
