import React, { useState, useEffect } from 'react';
import { Button, DatePicker, Select, Spin, Icon, Input } from 'antd';
import moment from 'moment';

const { Option } = Select;
const { RangePicker } = DatePicker;

interface Props {
    loggedTenant: string;
    togglingTenant: string;
    hasAccess: boolean;
    search: () => void;
    searchbyReferenceNo: () => void;
    loadRFOrder: () => void;
    inValidateConfirm: () => void;
    title: string;
}

const PsReportingComponent: React.FC<Props> = ({
    loggedTenant,
    togglingTenant,
    hasAccess,
    search,
    searchbyReferenceNo,
    loadRFOrder,
    inValidateConfirm,
    title
}) => {
    const [showDates, setShowDates] = useState(false);
    const [showRefNum, setShowRefNum] = useState(false);
    const [fromDate, setFromDate] = useState(moment());
    const [throughDate, setThroughDate] = useState(moment());
    const [referenceNo, setReferenceNo] = useState('');
    const [busy, setBusy] = useState(false);
    const [busyRef, setBusyRef] = useState(false);

    const handleDateChange = (dates: [moment.Moment, moment.Moment]) => {
        setFromDate(dates[0]);
        setThroughDate(dates[1]);
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
                            {togglingTenant === 'RF' && <Button onClick={() => search()}>Orders Summary</Button>}
                            {togglingTenant === 'LVIS' && <Button onClick={() => search('RF')}>RF Orders Summary</Button>}
                            {togglingTenant === 'RF' && <Button onClick={() => search('RF')}>RF Orders Summary</Button>}
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
                                <RangePicker
                                    disabled={busy}
                                    value={[fromDate, throughDate]}
                                    onChange={handleDateChange}
                                />
                                <Button onClick={handleSearchClick} disabled={busy}>
                                    {busy ? <Spin /> : <Icon type="search" />}
                                </Button>
                            </div>
                        )}
                        {showRefNum && (
                            <div>
                                <Input
                                    value={referenceNo}
                                    onChange={e => setReferenceNo(e.target.value)}
                                    disabled={busyRef}
                                />
                                <Button onClick={searchbyReferenceNo} disabled={busyRef}>
                                    {busyRef ? <Spin /> : <Icon type="search" />}
                                </Button>
                            </div>
                        )}
                        <div style={{ float: 'right' }}>
                            <Button disabled={!hasAccess || busy} onClick={inValidateConfirm}>
                                Invalidate Order
                            </Button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default PsReportingComponent;