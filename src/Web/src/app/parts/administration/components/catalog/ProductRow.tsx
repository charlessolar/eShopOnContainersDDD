import * as React from 'react';
import { ProductType } from '../../models/products';
import { Theme, WithStyles, withStyles } from '@material-ui/core/styles';
import TableRow from '@material-ui/core/TableRow';
import TableCell from '@material-ui/core/TableCell';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Typography from '@material-ui/core/Typography';
import Avatar from '@material-ui/core/Avatar';
import { Formatted, Using } from '../../../../components/models';
import ProductFormEdit from './ProductEdit';
import AvailableStockForm from '../availableStockForm';
import { CatalogStoreType } from '../../stores/catalog';

interface ProductRowProps {
  list: CatalogStoreType;
  product: ProductType;
}

const styles = (theme: Theme) => ({

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

class ProductRow extends React.PureComponent<ProductRowProps & WithStyles<'avatar' | 'row' | 'button'>> {

  public render() {
    const { classes, product, list } = this.props;

    return (
      <TableRow hover className={classes.row}>
        <Using model={product}>
          <TableCell component='th' scope='row'>
            <ListItem>
              <Avatar src={product.productPicture} className={classes.avatar} />
              <ListItemText primary={product.name} secondary={product.description} />
              <ProductFormEdit product={product} list={list} />
            </ListItem>
          </TableCell>
          <TableCell>{product.catalogType}</TableCell>
          <TableCell>{product.catalogBrand}</TableCell>
          <TableCell numeric><Formatted field='price' /></TableCell>
          <TableCell numeric>
            <Formatted field='availableStock' />
            <AvailableStockForm product={product} />
          </TableCell>
          <TableCell>
            <Typography variant='body2'>Restock: <Formatted field='restockThreshold' /></Typography>
            <Typography variant='body2'>Max Stock: <Formatted field='maxStockThreshold' /></Typography>
          </TableCell>
        </Using>
      </TableRow>
    );
  }
}

export default withStyles(styles)(ProductRow);
