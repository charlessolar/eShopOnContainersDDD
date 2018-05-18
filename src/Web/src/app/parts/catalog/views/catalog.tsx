import * as React from 'react';
import { observer } from 'mobx-react';
import { hot } from 'react-hot-loader';
import glamorous from 'glamorous';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Badge from '@material-ui/core/Badge';
import Typography from '@material-ui/core/Typography';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Button from '@material-ui/core/Button';
import IconButton from '@material-ui/core/IconButton';
import Grid from '@material-ui/core/Grid';
import Tooltip from '@material-ui/core/Tooltip';
import KeyboardArrowRight from '@material-ui/icons/KeyboardArrowRight';
import BasketIcon from '@material-ui/icons/ShoppingBasket';

import { inject, models } from '../../../utils';
import { Using, Field, Formatted } from '../../../components/models';

import { StoreType } from '../../../stores';
import { CatalogStoreModel, CatalogStoreType } from '../stores/catalog';

interface CatalogProps {
  store?: CatalogStoreType;
}

const styles = (theme: Theme) => ({
  flex: {
    flex: 1
  },
  navbar: {
    marginLeft: 50,
  },
  appbar: {
    boxShadow: '0 4px 2px -2px gray',
    backgroundColor: theme.palette.grey[100]
  },
  noProduct: {
    height: '80vh',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  },
  card: {
    width: 345,
  },
  media: {
    height: 0,
    paddingTop: '64.86%', // 370x240
  },
  button: {
    margin: theme.spacing.unit,
  },
  selectors: {
    display: 'flex',
    flex: 1
  },
  controls: {
    display: 'flex',
    maxWidth: '60vw'
  },
  dropdowns: {
    marginRight: 20
  },
  badge: {
  }
});

const AppView = glamorous('div')((_) => ({
  flex: '1 1 auto',
  width: '100vw',
  display: 'flex',
  flexDirection: 'column',
  overflow: 'auto',
  justifyContent: 'center',
  alignItems: 'center',
}));
const MainView = glamorous('main')({
  'width': '75vw',
  'flex': '1',
  'marginTop': 20,
  '@media(max-width: 600px)': {
    margin: 10
  }
});

@observer
class CatalogView extends React.Component<CatalogProps & WithStyles<'flex' | 'appbar' | 'navbar' | 'button' | 'noProduct' | 'media' | 'card' | 'selectors' | 'controls' | 'dropdowns' | 'badge'>, {}> {

  private pullProducts = () => {
    const { store } = this.props;
    store.get();
  }
  private openBasket = () => {
    const { store } = this.props;

    store.openBasket();
  }

  public render() {
    const { store, classes } = this.props;

    const products = Array.from(store.products.values());
    return (
      <AppView>
        <AppBar position='static' className={classes.appbar}>
          <Toolbar>
            <Using model={store}>
              <div className={classes.selectors}>
                <div className={classes.controls}>
                  <div className={classes.dropdowns}>
                    <Field field='catalogBrand' />
                  </div>
                  <div className={classes.dropdowns}>
                    <Field field='catalogType' />
                  </div>
                  <Button className={classes.button} variant='raised' size='small' color='primary' onClick={this.pullProducts}><KeyboardArrowRight /></Button>
                </div>
              </div>
              <IconButton className={classes.badge} onClick={this.openBasket}>
                <Badge color='primary' badgeContent={store.basketItems}>
                  <BasketIcon/>
                </Badge>
              </IconButton>
            </Using>
          </Toolbar>
        </AppBar>
        <MainView>
          {products.length === 0 ?
            <Grid container justify='center'>
              <Grid item xs={6}>
                <Typography variant='display3' className={classes.noProduct}>No products found</Typography>
              </Grid>
            </Grid>
            :
            <Grid container spacing={16} justify='flex-start' alignItems='flex-start'>
              {products.map((product, key) => (
                <Grid item xs key={key}>
                  <Using model={product}>
                    <Card className={classes.card}>
                      <CardMedia className={classes.media} title={product.name} image={product.productPicture || require('../img/placeholder.png')} />
                      <CardContent>
                        <Typography gutterBottom variant='headline' component='h2' className={classes.flex} noWrap>
                          {product.name}
                        </Typography>
                        <Formatted field='price' />
                        <Typography component='p'>
                          {product.description}
                        </Typography>
                      </CardContent>
                      <CardActions>
                        <Button color='primary' variant='raised' fullWidth className={classes.button} disabled={!product.canOrder} onClick={() => store.addToBasket(product)}>Add to Cart</Button>
                      </CardActions>
                    </Card>
                  </Using>
                </Grid>
              ))}
            </Grid>
          }
        </MainView>
      </AppView>
    );
  }
}

export default hot(module)(withStyles(styles)(CatalogView));
