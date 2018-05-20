import * as React from 'react';
import { observer } from 'mobx-react';
import { getSnapshot } from 'mobx-state-tree';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';

import { inject } from '../../../utils';
import { Using, Formatted, Field, Submit } from '../../../components/models';

import { AddressStoreType, AddressStoreModel } from '../stores/address';
import { CheckoutStoreType } from '../stores/checkout';

interface AddressProps {
  handleNext: () => void;
  handlePrev: () => void;

  store?: AddressStoreType;
  checkout: CheckoutStoreType;
}

const styles = (theme: Theme) => ({
  paper: {
    minHeight: 120,
    padding: 20
  },
  stepActions: {
    margin: 20,
    display: 'flex',
    justifyContent: 'flex-end',
  },
  stepButton: {
    alignSelf: 'flex-start'
  }
});

@inject(AddressStoreModel, 'store', 'checkout', (s: CheckoutStoreType) => {
  return {
    billingAddress: s.selectedBillingAddress || s.buyer.preferredAddressId,
    shippingAddress: s.selectedShippingAddress || s.buyer.preferredAddressId
  };
})
@observer
class AddressView extends React.Component<AddressProps & WithStyles<'paper' | 'stepActions' | 'stepButton'>, {}> {

  private handleNext = () => {
    const { store, checkout, handleNext } = this.props;

    checkout.selectBilling(getSnapshot(store.billingAddress));
    checkout.selectShipping(getSnapshot(store.shippingAddress));

    handleNext();
  }

  public render() {
    const { classes, store, handleNext, handlePrev } = this.props;

    return (
      <Using model={store}>
        <Grid container justify='center'>
          <Grid item xs={6}>
            <Paper className={classes.paper} elevation={2}>
              <Typography variant='headline' gutterBottom color='primary'>Billing Address</Typography>
              {store.billingAddress ? (
                <div>
                  <Typography variant='title'>{store.billingAddress.street}</Typography>
                  <Typography component='p'>
                    {store.billingAddress.city}, {store.billingAddress.state} {store.billingAddress.zipCode}<br />
                    {store.billingAddress.country}
                  </Typography>
                </div>
              ) : (<Typography variant='title'>( none )</Typography>)}
            </Paper>
          </Grid>
          <Grid item xs={4}>
            <Field field='billingAddress' />
          </Grid>
          <Grid item xs={6}>
            <Paper className={classes.paper} elevation={2}>
              <Typography variant='headline' gutterBottom color='primary'>Shipping Address</Typography>
              {store.shippingAddress ? (
                <div>
                  <Typography variant='title'>{store.shippingAddress.street}</Typography>
                  <Typography component='p'>
                    {store.shippingAddress.city}, {store.shippingAddress.state} {store.shippingAddress.zipCode}<br />
                    {store.shippingAddress.country}
                  </Typography>
                </div>
              ) : (<Typography variant='title'>( none )</Typography>)}
            </Paper>
          </Grid>
          <Grid item xs={4}>
            <Field field='shippingAddress' />
          </Grid>
        </Grid>

        <div className={classes.stepActions}>
          <Submit buttonProps={{variant: 'raised', className: classes.stepButton, color: 'primary'}} onSuccess={this.handleNext} text='Next' />
        </div>
      </Using>
    );
  }
}

export default withStyles(styles)<AddressProps>(AddressView);
