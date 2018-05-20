import * as React from 'react';

import { withStyles, WithStyles } from '@material-ui/core';
import IconButton from '@material-ui/core/IconButton';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import AddIcon from '@material-ui/icons/Add';

import { inject } from '../../../utils';
import { Using, Field, Submit } from '../../../components/models';

import { PaymentMethodFormType, PaymentMethodFormModel } from '../stores/paymentMethodForm';

const styles = theme => ({
  button: {
    margin: theme.spacing.unit,
  },
});

interface PaymentMethodFormProps {
  onChange: (newVal: any) => void;

  store?: PaymentMethodFormType;
}

@inject(PaymentMethodFormModel)
class PaymentMethodFormView extends React.Component<PaymentMethodFormProps & WithStyles<'button'>, {open: boolean}> {
  constructor(props) {
    super(props);

    this.state = {
      open: false
    };
  }

  private handleOpen = () => {
    this.setState({open: true});
  }
  private handleClose = () => {
    this.setState({open: false});
  }
  private handleSuccess = () => {
    const { onChange, store } = this.props;
    onChange({ id: store.id, alias: store.alias, cardNumber: store.cardNumber, securityNumber: store.securityNumber, cardholderName: store.cardholderName, expiration: store.expiration, cardType: store.cardType });
    this.handleClose();
  }

  public render() {
    const { classes, store } = this.props;

    return (
      <Using model={store}>
        <IconButton className={classes.button} onClick={this.handleOpen}><AddIcon/></IconButton>
        <Dialog
          open={this.state.open}
          onClose={this.handleClose}
          aria-labelledby='form-dialog-title'>
          <DialogTitle>Payment Method Create</DialogTitle>
          <DialogContent>
            <Field field='alias'/>
            <Field field='cardNumber'/>
            <Field field='securityNumber'/>
            <Field field='cardholderName'/>
            <Field field='expiration'/>
            <Field field='cardType'/>
          </DialogContent>
          <DialogActions>
            <Button onClick={this.handleClose} color='primary'>
              Cancel
            </Button>
            <Submit onSuccess={this.handleSuccess} buttonProps={{color: 'primary'}}/>
          </DialogActions>
        </Dialog>
      </Using>
    );
  }
}

export default withStyles(styles)<PaymentMethodFormProps>(PaymentMethodFormView);
