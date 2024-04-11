 
import React, { useState, useEffect } from 'react';
import { Modal, Button } from 'react-bootstrap';
import { useHistory } from 'react-router-dom';

interface Props {
  activityRight: string;
  canManageTEQ: boolean;
  canManageBEQ: boolean;
  getUser: () => Promise<{ ActivityRight: string; CanManageTEQ: boolean; CanManageBEQ: boolean }>;
  gridApi?: any; // Assuming gridApi is an object that handles grid operations, type should be defined based on actual usage
}

const AuditingComponent: React.FC<Props> = ({ activityRight, canManageTEQ, canManageBEQ, getUser, gridApi }) => {
  const [hasAccess, setHasAccess] = useState(false);
  const history = useHistory();

  useEffect(() => {
    if (!activityRight) {
      // Assuming activityRight is fetched and stored in cookies/localStorage
      const storedRight = localStorage.getItem('activityright');
      if (storedRight) {
        getUser().then(response => {
          // Broadcast equivalent in React would be setting state or context
          // Update state with new values
          setHasAccess(response.ActivityRight === 'Admin' || response.ActivityRight === 'SuperAdmin');
        }).catch(error => {
          console.error('Failed to fetch user data', error);
        });
      }
    } else {
      setHasAccess(activityRight === 'Admin' || activityRight === 'SuperAdmin');
    }
  }, [activityRight, getUser]);

  const handleModalClose = () => {
    history.push('/dashboard');
  };

  return (
    <div>
      {!hasAccess && (
        <Modal show={!hasAccess} onHide={handleModalClose}>
          <Modal.Header closeButton>
            <Modal.Title>Attention</Modal.Title>
          </Modal.Header>
          <Modal.Body>You are not authorized to view this page.</Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={handleModalClose}>
              Close
            </Button>
          </Modal.Footer>
        </Modal>
      )}
      {hasAccess && (
        <div>
          {/* Grid operations */}
          <button onClick={() => gridApi?.treeBase.expandAllRows()}>Expand All</button>
          <button onClick={() => gridApi?.treeBase.toggleRowTreeState(gridApi.grid.renderContainers.body.visibleRowCache[0])}>Toggle Row</button>
          <button onClick={() => {
            gridApi?.grouping.clearGrouping();
            gridApi?.grouping.groupColumn('Name');
          }}>Change Grouping</button>
        </div>
      )}
    </div>
  );
};

export default AuditingComponent;
