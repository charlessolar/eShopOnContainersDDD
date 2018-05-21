import * as React from 'react';
import { observer } from 'mobx-react';
import { hot } from 'react-hot-loader';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Button from '@material-ui/core/Button';
import Divider from '@material-ui/core/Divider';
import Typography from '@material-ui/core/Typography';
import Avatar from '@material-ui/core/Avatar';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Tooltip from '@material-ui/core/Tooltip';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import KeyboardArrowRight from '@material-ui/icons/KeyboardArrowRight';
import NotificationIcon from '@material-ui/icons/Notifications';

import { sort } from '../../../utils';
import { Using, Formatted, Field } from '../../../components/models';
import { OrderStoreType, OrderStoreModel } from '../stores/orders';

interface OrderProps {
  store?: OrderStoreType;
}

const styles = (theme: Theme) => ({
  appView: {
    flex: '1 1 auto',
    width: '100vw',
    display: 'flex',
    flexDirection: 'column',
    overflow: 'auto',
    justifyContent: 'center',
    alignItems: 'center',
  },
  mainView: {
    'width': '75vw',
    'flex': '1',
    'marginTop': 20,
    '@media(max-width: 600px)': {
      margin: 10
    }
  },
  root: {
    marginTop: theme.spacing.unit * 3,
    width: '100%',
  },
  container: {
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto',
  },
  noOrders: {
    height: '80vh',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  },
  navbar: {
    marginLeft: 50,
  },
  appbar: {
    boxShadow: '0 4px 2px -2px gray',
    backgroundColor: theme.palette.grey[100]
  },
  table: {
  },
  avatar: {
    margin: 10,
    width: 80,
    height: 80
  },
  row: {
    '&:nth-of-type(odd)': {
      backgroundColor: theme.palette.background.default,
    },
  },
  details: {
    minHeight: 200,
  },
  avatarQuantity: {
    backgroundColor: theme.palette.primary.main,
    color: theme.palette.primary.contrastText,
    margin: theme.spacing.unit
  },
  divider: {
    marginBottom: 10,
  },
  price: {
    color: theme.palette.error[900]
  },
  button: {
    margin: theme.spacing.unit,
  },
  selectors: {
    display: 'flex',
    flex: 1
  },
  dropdowns: {
    marginRight: 20,
  },
  controls: {
    display: 'flex',
    maxWidth: '60vw'
  },
  status: {
    textDecorationLine: 'underline',
    textDecorationStyle: 'dashed',
    textDecorationColor: theme.palette.primary.main,
  }
});

const HeaderTableCell = withStyles(theme => ({
  head: {
    backgroundColor: theme.palette.grey[300],
    color: theme.palette.grey[700],
    fontSize: 14,
  },
}))(TableCell as any);

@observer
class OrderView extends React.Component<OrderProps & WithStyles<'appView' | 'mainView' | 'root' | 'container' | 'navbar' | 'appbar' | 'noOrders' | 'table' | 'avatar' | 'row' | 'details' | 'product' | 'avatarQuantity' | 'divider' | 'price' | 'button' | 'selectors' | 'controls' | 'dropdowns' | 'status'>, {}> {

  private pullOrders = () => {
    const { store } = this.props;
    store.get();
  }

  public render() {
    const { store, classes } = this.props;

    const orders = sort(Array.from(store.orders.values()), 'created', 'desc');

    return (
      <div className={classes.appView}>
        <AppBar position='static' className={classes.appbar}>
          <Toolbar>
            <Using model={store}>
              <Grid container>
                <Grid item xs={3}>
                    <Field field='orderStatus' />
                  </Grid>
                  <Grid item xs={2}>
                    <Field field='period' />
                  </Grid>
                  <Grid item xs={1}>
                  <Button className={classes.button} variant='raised' size='small' color='primary' onClick={this.pullOrders}><KeyboardArrowRight /></Button>
                  </Grid>
              </Grid>
            </Using>
          </Toolbar>
        </AppBar>
        <main className={classes.mainView}>
          {orders.length === 0 ?
            <Grid container justify='center'>
              <Grid item xs={6}>
                <Typography variant='display3' className={classes.noOrders}>No orders found</Typography>
              </Grid>
            </Grid>
            :
              orders.map((order) => (
                <Using model={order} key={order.id}>
                  <Table className={classes.table}>
                    <TableHead>
                      <TableRow>
                        <HeaderTableCell><strong>Date Placed</strong><br />{order.placed}</HeaderTableCell>
                        <HeaderTableCell><strong>Total</strong><br /><Formatted field='total' /></HeaderTableCell>
                        <HeaderTableCell><strong>Ship To</strong><br />{order.shippingAddress}</HeaderTableCell>
                        <HeaderTableCell><strong>Order #</strong><br />{order.id}</HeaderTableCell>
                        <HeaderTableCell><strong>Status</strong><br /><Tooltip title={order.statusDescription}><span className={classes.status}>{order.status}</span></Tooltip></HeaderTableCell>
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      <TableRow>
                        <TableCell colSpan={5}>
                          <Typography variant='headline'>Order Items</Typography>
                            {order.items.map((item) => (
                              <Using model={item} key={item.id}>
                              <Grid container>
                                <Grid item xs={10}>
                                  <Grid container>
                                    <Grid item xs={2}>
                                        <Avatar src={item.productPicture} className={classes.avatar} />
                                    </Grid>
                                    <Grid item xs={4}>
                                        <Typography variant='title'>{item.productName}</Typography>
                                        <Typography variant='subheading'>{item.productDescription}</Typography>
                                        <Typography variant='subheading' className={classes.price}><Formatted field='itemPrice'/></Typography>
                                    </Grid>
                                    <Grid item xs={2}>
                                      <Avatar className={classes.avatarQuantity}>{item.quantity}</Avatar>
                                    </Grid>
                                    <Grid item xs={2}>
                                      <Typography variant='headline' className={classes.price}><Formatted field='subTotal'/></Typography>
                                    </Grid>
                                    <Grid item xs={2}>
                                      <Typography variant='title'>Total</Typography>
                                      <Typography variant='headline' className={classes.price}><Formatted field='total'/></Typography>
                                    </Grid>
                                  </Grid>
                                </Grid>
                                <Grid item xs={2}>
                                  <Button fullWidth color='primary' variant='raised' disabled>Return Item</Button>
                                </Grid>
                              </Grid>
                              <Divider className={classes.divider}/>
                              </Using>
                            ))}
                        </TableCell>
                      </TableRow>
                    </TableBody>
                  </Table>
                </Using>
              ))
          }
        </main>
      </div>
    );
  }
}

export default hot(module)(withStyles(styles as any)<OrderProps>(OrderView));
