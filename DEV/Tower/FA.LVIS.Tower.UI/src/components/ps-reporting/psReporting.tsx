 
import React, { useState, useEffect } from 'react';
import { Table, Button, Modal } from 'antd';
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
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [modalContent, setModalContent] = useState<RowData | null>(null);

    const handleSelectChange = (selectedRowKeys: React.Key[], selectedRows: RowData[]) => {
        setSelectedRowKeys(selectedRowKeys);
        onRowSelectionChange(selectedRowKeys, selectedRows);
    };

    const handleRowDoubleClick = (record: RowData) => {
        setModalContent(record);
        setIsModalVisible(true);
    };

    const handleModalClose = () => {
        setIsModalVisible(false);
        setModalContent(null);
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
                onRow={(record) => ({
                    onDoubleClick: () => handleRowDoubleClick(record),
                })}
                rowSelection={rowSelection}
                columns={columns}
                dataSource={rows.filter(row => row.InternalRefNum === activeCustomerName)}
                rowKey="ServiceRequestId"
            />
            <Modal
                title="Detail View"
                visible={isModalVisible}
                onCancel={handleModalClose}
                footer={null}
            >
                {modalContent && (
                    <div>
                        <p>Service Request ID: {modalContent.ServiceRequestId}</p>
                        <p>Order Date: {new Date(modalContent.createddate).toLocaleDateString('en-US')}</p>
                        <p>Service Type: {modalContent.service}</p>
                        <p>Application ID: {modalContent.ApplicationId}</p>
                        <p>Customer ID: {modalContent.CustomerId}</p>
                        <p>External Reference Number: {modalContent.ExternalRefNum}</p>
                        <p>Customer Reference Number: {modalContent.CustomerRefNum}</p>
                        <p>Internal Reference Number: {modalContent.InternalRefNum}</p>
                        <p>Internal Reference ID: {modalContent.InternalRefId}</p>
                    </div>
                )}
            </Modal>
        </div>
    );
};

export default ReportingComponent;
