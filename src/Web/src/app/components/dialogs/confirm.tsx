import * as React from 'react';
import { Theme, WithStyles, withStyles } from 'material-ui/styles';
import Button from 'material-ui/Button';
import Dialog, {
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from 'material-ui/Dialog';

interface ConfirmProps {
  title: string;
  description: string;

}

const styles = (theme: Theme) => ({

});

class ConfirmationDialog extends React.Component<ConfirmProps & WithStyles<never>, { open: boolean, confirm: () => void }> {
  constructor(props) {
    super(props);
    this.state = {
      open: false,
      confirm: undefined
    };
  }

  private handleClose = () => {
    this.setState({open: false});
  }
  private handleOpen = (confirm: () => void) => {
    this.setState({open: true, confirm});
  }
  private handleConfirm = () => {
    this.state.confirm();

    this.handleClose();
  }
  private handleCancel = () => {
    this.handleClose();
  }

  public render() {
    const { title, description, children } = this.props;

    // add dialog hook
    const mapped = React.Children.map(children, (child) => React.cloneElement(child as any, { dialogOpen: this.handleOpen }));

    return (
      <>
        {mapped}
        <Dialog
          disableBackdropClick
          disableEscapeKeyDown
          maxWidth='xs'
          aria-labelledby='confirmation-dialog-title'
          aria-describedby='alert-dialog-description'
          open={this.state.open}
          >
          <DialogTitle id='confirmation-dialog-title'>{title}</DialogTitle>
          <DialogContent>
            <DialogContentText id='alert-dialog-description'>
              {description}
            </DialogContentText>
          </DialogContent>
          <DialogActions>
            <Button onClick={this.handleCancel} color='primary'>
              Cancel
            </Button>
            <Button onClick={this.handleConfirm} color='primary'>
              Ok
            </Button>
          </DialogActions>
        </Dialog>
      </>
    );
  }
}

export default withStyles(styles)<ConfirmProps>(ConfirmationDialog);
