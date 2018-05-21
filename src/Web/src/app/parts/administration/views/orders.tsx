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

import { sort } from '../../../utils';
import { Using, Formatted } from '../../../components/models';
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
  }
});

@observer
class OrdersView extends React.Component<OrdersProps & WithStyles<'root' | 'container' | 'table' | 'avatar' | 'row' | 'button'>, {}> {

  public render() {
    const { store, classes } = this.props;

    const orders = sort(Array.from(store.orders.values()), 'created', 'desc');

    return (
      <div className={classes.root}>
      <Grid container justify='center'>
        <Grid item xs={8}>
        <Paper className={classes.container}>
            <Table className={classes.table}>
              <TableHead>
                <TableRow>
                  <TableCell>Created</TableCell>
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
                        <Typography variant='subheading'>{order.placed}</Typography>
                        <Typography variant='body1' color='textSecondary'>{order.status}</Typography>
                      </TableCell>
                      <TableCell>{order.buyerName}</TableCell>
                      <TableCell>
                        <Typography variant='subheading'>{order.shippingAddress}</Typography>
                        <Typography variant='body1' color='textSecondary' paragraph>{order.shippingCityState} {order.shippingZipCode}<br/>{order.shippingCountry}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography variant='subheading'>{order.billingAddress}</Typography>
                        <Typography variant='body1' color='textSecondary' paragraph>{order.billingCityState} {order.billingZipCode}<br/>{order.billingCountry}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography variant='subheading'>{order.paymentMethod}</Typography>
                      </TableCell>
                      <TableCell numeric>
                        <Typography variant='subheading'><Formatted field='subTotal' /></Typography>
                        <Typography variant='body1' color='textSecondary'>Quantity: {order.totalQuantity} Items: {order.totalItems}</Typography>
                      </TableCell>
                      <TableCell>
                        <Typography variant='subheading'><Formatted field='total' /></Typography>
                        <Typography variant='body1'>Additional: <Formatted field='additional' /></Typography>
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
