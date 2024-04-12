import React, { useState } from 'react';
import { Button, DatePicker, Select, Icon } from 'antd';
import 'antd/dist/antd.css';

interface Props {
    loggedTenant: string;
    togglingTenant: string;
    title: string;
    hasAccess: boolean;
    inValidBtnEnable: boolean;
    orderToInvalidate: any[];
    activityright: string;
    canmanageteq: boolean;
    canmanagebeq: boolean;
    getUser: (response: any) => void;
    invalidateOrder: () => void;
    search: () => void;
    loadRFOrder: () => void;
    searchbyReferenceNo: () => void;
}

const { Option } = Select;

const ReportingComponent: React.FC<Props> = ({
    loggedTenant,
    togglingTenant,
    title,
    hasAccess,
    inValidBtnEnable,
    orderToInvalidate,
    activityright,
    canmanageteq,
    canmanagebeq,
    getUser,
    invalidateOrder,
    search,
    loadRFOrder,
    searchbyReferenceNo
}) => {
    const [showDates, setShowDates] = useState(false);
    const [showRefNum, setShowRefNum] = useState(false);
    const [fromDate, setFromDate] = useState(null);
    const [throughDate, setThroughDate] = useState(null);
    const [referenceNo, setReferenceNo] = useState('');

    const handleDateChange = (value: any, dateString: string, key: 'fromDate' | 'throughDate') => {
        if (key === 'fromDate') {
            setFromDate(dateString);
        } else {
            setThroughDate(dateString);
        }
    };

    const handleSearchClick = () => {
        if (togglingTenant === 'RF') {
            loadRFOrder();
        } else {
            search();
        }
    };

    return (
        <div className="ps-dashboard-header">
            <ul className="breadcrumb">
                <li className="subbreadcrumb">
                    {loggedTenant === 'LVIS' ? (
                        <>
                            {togglingTenant === 'LVIS' && <Button onClick={() => search()}>Orders Summary</Button>}
                            {togglingTenant === 'RF' && <Button onClick={() => search()}>RF Orders Summary</Button>}
                        </>
                    ) : (
                        <Button onClick={() => search()}>Orders Summary</Button>
                    )}
                </li>
            </ul>
            <div className="wrapper">
                <div className="col-lg-10">
                    <ul className="ps-page-title">
                        <li>{title}</li>
                    </ul>
                    <div className="form-group">
                        <Icon type="calendar" onClick={() => setShowDates(!showDates)} />
                        <Icon type="bars" onClick={() => setShowRefNum(!showRefNum)} />
                        {showDates && (
                            <div>
                                <DatePicker onChange={(date, dateString) => handleDateChange(date, dateString, 'fromDate')} />
                                <DatePicker onChange={(date, dateString) => handleDateChange(date, dateString, 'throughDate')} />
                                <Button onClick={handleSearchClick}>Search</Button>
                            </div>
                        )}
                        {showRefNum && (
                            <div>
                                <Select onChange={(value: string) => setReferenceNo(value)}>
                                    <Option value="ref1">Reference 1</Option>
                                    <Option value="ref2">Reference 2</Option>
                                </Select>
                                <Button onClick={searchbyReferenceNo}>Search by Reference No</Button>
                            </div>
                        )}
                        <Button disabled={inValidBtnEnable} onClick={invalidateOrder}>Invalidate Order</Button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ReportingComponent;
