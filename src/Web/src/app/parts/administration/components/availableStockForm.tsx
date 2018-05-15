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

import { ProductType } from '../models/products';
import { StockFormType, StockFormModel } from '../stores/availableStock';

const styles = (theme: Theme) => ({
  button: {
    margin: theme.spacing.unit,
    color: theme.palette.primary.light
  },
});

interface StockFormProps {
  product: ProductType;

  store?: StockFormType;
}

@inject(StockFormModel, 'store', 'product')
class StockFormView extends React.Component<StockFormProps & WithStyles<'button'>, { open: boolean }> {
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
    const { product, store } = this.props;
    product.updateStock(store.availableStock);
    this.handleClose();
  }

  public render() {
    const { classes, store } = this.props;

    return (
      <Using model={store}>
        <Tooltip title='Modify Stock'>
          <IconButton className={classes.button} onClick={this.handleOpen} aria-label='Modify Stock'><AssignmentIcon /></IconButton>
        </Tooltip>
        <Dialog
          open={this.state.open}
          onClose={this.handleClose}
          aria-labelledby='form-dialog-title'>
          <DialogTitle>Modify Stock</DialogTitle>
          <DialogContent>
            <Field field='availableStock' />
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

export default withStyles(styles)<StockFormProps>(StockFormView);
