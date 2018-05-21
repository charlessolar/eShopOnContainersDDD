import * as React from 'react';
import { observer } from 'mobx-react';
import { hot } from 'react-hot-loader';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';
import Tooltip from '@material-ui/core/Tooltip';
import IconButton from '@material-ui/core/IconButton';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import Avatar from '@material-ui/core/Avatar';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import KeyboardArrowRight from '@material-ui/icons/KeyboardArrowRight';

import { sort } from '../../../utils';
import { Using, Formatted, Field } from '../../../components/models';
import { OrdersStoreType, OrdersStoreModel } from '../stores/orders';

interface OrdersProps {
  store?: OrdersStoreType;
}

const styles = (theme: Theme) => ({
  root: {
    marginTop: theme.spacing.unit * 3,
    width: '100%',
  },
  container: {
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto',
    padding: 10
  },
  table: {
  },
  avatar: {
    margin: 10,
    width: 60,
    height: 60
  },
  row: {
    '&:nth-of-type(odd)': {
      backgroundColor: theme.palette.background.default,
    },
  },
  button: {
    margin: theme.spacing.unit,
    color: theme.palette.primary.light
  },
  address: {
    fontSize: '0.70em'
  }
});

@observer
class OrdersView extends React.Component<OrdersProps & WithStyles<'root' | 'container' | 'table' | 'avatar' | 'row' | 'button' | 'address'>, {}> {

  private pullOrders = () => {
    const { store } = this.props;
    store.get();
  }

  public render() {
    const { store, classes } = this.props;

    const orders = sort(Array.from(store.orders.values()), 'created', 'desc');

    return (
      <div className={classes.root}>
      <Grid container justify='center'>
        <Grid item xs={8}>
        <Paper className={classes.container}>
          <Using model={store}>
            <Field field='orderStatus'/>
            <Button className={classes.button} variant='raised' size='small' color='primary' onClick={this.pullOrders}><KeyboardArrowRight /></Button>
          </Using>

            <Table className={classes.table}>
              <TableHead>
                <TableRow>
                  <TableCell>Status</TableCell>
                  <TableCell>Buyer</TableCell>
                  <TableCell>Destination</TableCell>
                  <TableCell>Billing</TableCell>
                  <TableCell>Payment</TableCell>
                  <TableCell>SubTotal</TableCell>
                  <TableCell>Total</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
              {orders.map(order => (
                  <TableRow hover key={order.id} className={classes.row}>
                    <Using model={order}>
                      <TableCell component='th' scope='row'>
                        <Typography variant='caption'>{order.placed}</Typography>
                        {order.status}
                      </TableCell>
                      <TableCell>{order.buyerName}</TableCell>
                      <TableCell>
                        <Typography variant='caption'>{order.shippingAddress}</Typography>
                        <Typography className={classes.address} color='textSecondary' paragraph>{order.shippingCityState} {order.shippingZipCode}<br/>{order.shippingCountry}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography variant='caption'>{order.billingAddress}</Typography>
                        <Typography className={classes.address} color='textSecondary' paragraph>{order.billingCityState} {order.billingZipCode}<br/>{order.billingCountry}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography variant='body1'>{order.paymentMethod}</Typography>
                      </TableCell>
                      <TableCell numeric>
                        <Typography variant='subheading'><Formatted field='subTotal' /></Typography>
                        <Typography variant='body1' color='textSecondary'>Qty: {order.totalQuantity}<br />Itms: {order.totalItems}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography variant='subheading'><Formatted field='total' /></Typography>
                        <Typography variant='body1' color='textSecondary'>+ <Formatted field='additional' /></Typography>
                      </TableCell>
                    </Using>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </Paper>
        </Grid>
      </Grid>
      </div>
      );
  }
}

export default hot(module)(withStyles(styles as any)(OrdersView));
