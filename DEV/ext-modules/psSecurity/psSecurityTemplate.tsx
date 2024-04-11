import React, { useState, useEffect } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

interface UserInfo {
    ActivityRight: string;
    CanManageTEQ: boolean;
    CanManageBEQ: boolean;
}

interface Props {
    getUserInfo: () => Promise<UserInfo>;
    deleteUser: (user: any) => Promise<number>;
    serviceGridData: any[];
    setServiceGridData: (data: any[]) => void;
}

const SecurityController: React.FC<Props> = ({ getUserInfo, deleteUser, serviceGridData, setServiceGridData }) => {
    const [userPermissions, setUserPermissions] = useState<UserInfo | null>(null);
    const [hasAccess, setHasAccess] = useState(false);
    const [hasModifyAccess, setHasModifyAccess] = useState(false);
    const history = useHistory();

    useEffect(() => {
        const fetchUserPermissions = async () => {
            const userInfo = await getUserInfo();
            setUserPermissions(userInfo);
            if (userInfo.ActivityRight !== 'Admin' && userInfo.ActivityRight !== 'SuperAdmin') {
                showModal();
            } else {
                if (userInfo.ActivityRight === 'Admin' || userInfo.ActivityRight === 'SuperAdmin') {
                    setHasAccess(true);
                }
                if (userInfo.ActivityRight === 'SuperAdmin') {
                    setHasModifyAccess(true);
                }
            }
        };

        fetchUserPermissions();
    }, []);

    const showModal = () => {
        return (
            <Modal show={true} onHide={() => history.push('/dashboard')} size="sm">
                <Modal.Header closeButton>
                    <Modal.Title>Attention</Modal.Title>
                </Modal.Header>
                <Modal.Body>You are not authorized to view this page.</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => history.push('/dashboard')}>
                        Close
                    </Button>
                </Modal.Footer>
            </Modal>
        );
    };

    const addRow = () => {
        const newService = {
            ID: "",
            UserId: "",
            Role: "",
            IsActive: "false",
            EmailId: "",
            Employeeid: "",
            sTenant: "",
            ManageBEQ: false,
            ManageTEQ: false
        };
        setServiceGridData([...serviceGridData, newService]);
    };

    const deleteRow = async (row: any) => {
        const confirmDelete = window.confirm(`Are you sure you want to delete "${row.Name}" user profile?`);
        if (confirmDelete) {
            const result = await deleteUser(row);
            if (result === 0) {
                toast.error('Cannot Delete row');
            } else {
                const updatedGridData = serviceGridData.filter(item => item !== row);
                setServiceGridData(updatedGridData);
                toast.success('The record was deleted successfully');
            }
        }
    };

    return (
        <div>
            {userPermissions && !hasAccess && showModal()}
            <ToastContainer />
            <div>
                {hasAccess && (
                    <button onClick={addRow}>Add Row</button>
                )}
                {serviceGridData.map((row, index) => (
                    <div key={index}>
                        <span>{row.Name}</span>
                        {hasModifyAccess && (
                            <button onClick={() => deleteRow(row)}>Delete Row</button>
                        )}
                    </div>
                ))}
            </div>
        </div>
    );
};

export default SecurityController;