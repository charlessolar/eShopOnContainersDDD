import * as React from 'react';

import { Theme, withStyles, WithStyles } from 'material-ui';
import IconButton from 'material-ui/IconButton';
import Button from 'material-ui/Button';
import Tooltip from 'material-ui/Tooltip';
import Dialog, {
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from 'material-ui/Dialog';
import AssignmentIcon from '@material-ui/icons/Assignment';

import { inject } from '../../../utils';
import { Using, Field, Submit } from '../../../components/models';

import { ItemIndexType } from '../models/items';
import { QuantityFormType, QuantityFormModel } from '../stores/quantity';

const styles = (theme: Theme) => ({
  button: {
    margin: theme.spacing.unit,
    color: theme.palette.primary.light
  },
});

interface QuantityFormProps {
  item: ItemIndexType;

  store?: QuantityFormType;
}

@inject(QuantityFormModel, 'store', 'item')
class QuantityFormView extends React.Component<QuantityFormProps & WithStyles<'button'>, { open: boolean }> {
  constructor(props) {
    super(props);

    this.state = {
      open: false
    };
  }

  private handleOpen = () => {
    this.setState({ open: true });
  }
  private handleClose = () => {
    this.setState({ open: false });
  }
  private handleSuccess = () => {
    const { item, store } = this.props;
    item.updateQuantity(store.quantity);
    this.handleClose();
  }

  public render() {
    const { classes, store } = this.props;

    return (
      <Using model={store}>
        <Tooltip title='Modify Quantity'>
          <IconButton className={classes.button} onClick={this.handleOpen} aria-label='Modify Quantity'><AssignmentIcon /></IconButton>
        </Tooltip>
        <Dialog
          open={this.state.open}
          onClose={this.handleClose}
          aria-labelledby='form-dialog-title'>
          <DialogTitle>Modify Quantity</DialogTitle>
          <DialogContent>
            <Field field='quantity' />
          </DialogContent>
          <DialogActions>
            <Button onClick={this.handleClose} color='primary'>
              Cancel
            </Button>
            <Submit onSuccess={this.handleSuccess} buttonProps={{ color: 'primary' }} />
          </DialogActions>
        </Dialog>
      </Using>
    );
  }
}

export default withStyles(styles)<QuantityFormProps>(QuantityFormView);
