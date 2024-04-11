 
import React, { useState, useEffect } from 'react';
import { Modal, Button } from 'react-bootstrap';
import axios from 'axios';
import Cookies from 'js-cookie';

interface Props {
  activityRight: string;
  canManageTEQ: boolean;
  canManageBEQ: boolean;
  getUser: () => Promise<any>;
  tenantName: string;
}

const PsReportingComponent: React.FC<Props> = ({ activityRight, canManageTEQ, canManageBEQ, getUser, tenantName }) => {
  const [orderToInvalidate, setOrderToInvalidate] = useState([]);
  const [inValidBtnEnable, setInValidBtnEnable] = useState(true);
  const [hasAccess, setHasAccess] = useState(false);
  const [hasSuperAccess, setHasSuperAccess] = useState(false);

  useEffect(() => {
    const checkAccess = () => {
      if (activityRight === 'Admin' || activityRight === 'SuperAdmin') {
        setHasAccess(true);
      }
      if (activityRight === 'SuperAdmin') {
        setHasSuperAccess(true);
      }
    };

    if (!activityRight) {
      const storedActivityRight = Cookies.get('activityright');
      if (storedActivityRight) {
        checkAccess();
      } else {
        getUser().then(response => {
          checkAccess();
        }).catch(error => {
          console.error('Failed to fetch user data', error);
        });
      }
    } else {
      checkAccess();
    }
  }, [activityRight, getUser]);

  return (
    <div>
      <h1>Reporting Dashboard</h1>
      <div>
        Tenant: {tenantName}
      </div>
      <Button disabled={!inValidBtnEnable} onClick={() => setOrderToInvalidate([])}>
        Invalidate Orders
      </Button>
      <div>
        Access Level: {hasAccess ? 'Admin/SuperAdmin' : 'User'}
      </div>
      <div>
        Super Access: {hasSuperAccess ? 'Yes' : 'No'}
      </div>
    </div>
  );
};

export default PsReportingComponent;
