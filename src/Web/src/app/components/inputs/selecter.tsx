import * as React from 'react';
import { observable, action, computed } from 'mobx';
import { observer, inject } from 'mobx-react';
import { types, flow, getEnv, IModelType, getSnapshot, getIdentifier, IStateTreeNode } from 'mobx-state-tree';
import * as keycode from 'keycode';
import Downshift from 'downshift';
import Debug from 'debug';

import { withStyles, WithStyles } from '@material-ui/core/styles';
import Input from '@material-ui/core/Input';
import InputLabel from '@material-ui/core/InputLabel';
import InputAdornment from '@material-ui/core/InputAdornment';
import FormControl from '@material-ui/core/FormControl';
import FormHelperText from '@material-ui/core/FormHelperText';
import MenuItem from '@material-ui/core/MenuItem';
import TextField from '@material-ui/core/TextField';
import Paper from '@material-ui/core/Paper';
import Chip from '@material-ui/core/Chip';
import IconButton from '@material-ui/core/IconButton';
import Close from '@material-ui/icons/Close';
import ArrowDown from '@material-ui/icons/KeyboardArrowDown';

import { models, inject_props } from '../../utils';

const debug = new Debug('selecter');

const renderSuggestion = ({ suggestion, index, itemProps, highlightedIndex, selectedItem }) => {
  const isHighlighted = highlightedIndex === index;
  const isSelected = selectedItem && selectedItem === suggestion.label;

  return (
    <MenuItem
      {...itemProps}
      key={suggestion.id}
      selected={isHighlighted}
      component='div'
      style={{
        fontWeight: isSelected ? 'bold' : 'normal',
      }}
    >
      {suggestion.label}
    </MenuItem>
  );
};

interface SelectStore {

  entries: Map<string, any>;
  loading: boolean;

  list: (term: string, id?: string) => Promise<{}>;
  clear: () => void;
  add: (model: any) => void;
  readonly projection: { id: string, label: string }[];
}

interface SelectProps {
  id: string;
  required?: boolean;
  label: string;
  error?: any;
  type?: string;
  value?: any;
  disabled?: boolean;
  onChange?: (newVal: any) => void;
  fieldProps?: any;

  selectStore?: IModelType<any, SelectStore>;

  addComponent?: React.ComponentType<{ onChange: (newVal: any) => void }>;

  // injected
  store?: SelectType;
}

interface SelectType {
  selected?: string;
  readonly selection: { id: string, label: string };
  term: string;

  readonly loading: boolean;
  readonly records: {id: string, label: string}[];

  list: (id?: string) => Promise<{}>;
  updateTerm: (newVal: string) => Promise<{}>;
  reset: () => Promise<{}>;
  selectItem: (id: string) => Promise<{}>;
  findById: (id: string) => {id: string, label: string};
  modelById: (id: string) => any;
  addModel: (model: any) => void;
}
const selectModel = types
  .model(
    {
      selected: types.maybe(types.string),
      term: types.optional(types.string, ''),
    })
    .views(self => ({
      get loading() {
        const select = getEnv(self).store as SelectStore;
        return select.loading;
      },
      get records() {
        const select = getEnv(self).store as SelectStore;
        return select.projection;
      }
    }))
  .actions(self => {
    const list: (id?: string) => Promise<{}> = flow(function*(id?: string) {
      const select = getEnv(self).store as SelectStore;

      try {
        yield select.list(self.term, id);
      } catch (e) {
        debug('error fetching: ', e);
      }
    });
    const updateTerm = flow(function*(newVal: string) {
      self.term = newVal;
      yield list();
    });
    const reset = flow(function*() {
      const select = getEnv(self).store as SelectStore;
      select.clear();
      self.selected = '';
      self.term = '';
      yield list();
    });
    const selectItem = flow(function*(id: string | IStateTreeNode) {
      // when a value is already set, we need to 'select' it to display in the control
      // the existing value could be a string Id or a model.  Assume an 'id' value exists
      if (typeof id !== 'string') {
        id = getIdentifier(id);
      }
      if (!findById(id)) {
        yield list(id);
      }
      self.selected = id;
    });
    const findById = (id: string) => {
      return self.records.find(x => x.id === id);
    };
    const modelById = (id: string) => {
      const select = getEnv(self).store as SelectStore;
      return select.entries.get(id);
    };
    const addModel = (model: any) => {
      const select = getEnv(self).store as SelectStore;
      select.add(model);
    };
    return { findById, modelById, selectItem, list, updateTerm, reset, addModel };
  })
  .views(self => ({
    get selection() {
      if (!self.selected) {
        return undefined;
      }
      const model = self.findById(self.selected);
      return model == null ? undefined : model;
    }
  }));

  // Double observer to observe the props received and the props used by the component (weird I know)
@observer
@inject((stores, props: SelectProps) => {
  const modelStore = props.selectStore.create({}, { api: stores['store'].api });

  props['store'] = selectModel.create({}, { store: modelStore });
  return props;
})
@observer
class IntegrationDownshift extends React.Component<SelectProps & WithStyles<'paper' | 'root' | 'container' | 'formControl'>, {}> {

  private _value: string;
  private _clearSelection: () => void;

  public componentDidMount() {
    const { value, store } = this.props;
    if (value) {
      store.selectItem(value);
    }
  }
  public componentDidUpdate() {
    const { value, store } = this.props;
    if (!value && this._value) {
      this._value = undefined;
      this._clearSelection();
    }
    if (value) {
      const snapshot = getSnapshot(value);
      store.addModel(snapshot);
      store.selectItem(value);
    }
  }

  // Called when input element changes
  private onInput = (e: any) => {
    const { store, onChange } = this.props;

    const newVal = e.target.value;
    const selectedValue = this._value && store.findById(this._value);
    if (selectedValue && selectedValue.label !== newVal) {
      onChange(undefined);
    }

    store.updateTerm(newVal);
  }
  // Called when downshift selects something
  private selectionChanged = (selection?: {id: string, label: string}) => {
    const { store, onChange } = this.props;

    if (!selection) {
      this._value = undefined;
      onChange(undefined);
      store.reset();
      this._clearSelection();
      return;
    }

    onChange(getSnapshot(store.modelById(selection.id)));
    store.selectItem(selection.id);
    this._value = selection.id;
  }
  private modelAdded = (model: any) => {
    const { store, onChange } = this.props;
    onChange(model);
    store.addModel(model);
    const selected = store.records[0];
    this._value = selected.id;
  }
  private openMenu = async (open: () => void) => {
    const { store } = this.props;
    await store.list();
    open();
  }

  private _renderDownshift = (classes, error, required, label, id, fieldProps, store, value, disabled, addComponent) => ({ getInputProps, getItemProps, isOpen, inputValue, selectedItem, highlightedIndex, clearSelection, openMenu }) => {
    this._clearSelection = clearSelection;

    return (
      <div className={classes.container}>

        <FormControl required={required} className={classes.formControl} fullWidth disabled={disabled} error={error && error[id] ? true : false} aria-describedby={id + '-text'}>
          <InputLabel htmlFor={id}>{label}</InputLabel>
          <Input id={id} onInput={this.onInput} type='text' fullWidth {...fieldProps} {...getInputProps()} value={inputValue || ''} endAdornment={
            value ?
              <InputAdornment position='end'>
                <IconButton
                  aria-label='Clear'
                  onClick={() => this.selectionChanged()}
                  disabled={disabled}
                >
                  <Close />
                </IconButton>
              </InputAdornment>
              :
              <InputAdornment position='end'>
                <IconButton
                  aria-label='Open'
                  onClick={() => this.openMenu(openMenu)}
                  disabled={disabled}
                >
                  <ArrowDown />
                </IconButton>
                {addComponent && !disabled && React.createElement(addComponent, { onChange: this.modelAdded })}
              </InputAdornment>
          } />
          {error && error[id] ? error[id].map((e, key) => (<FormHelperText key={key} id={id + '-' + key + '-text'}>{e}</FormHelperText>)) : undefined}
        </FormControl>
        {isOpen ? (
          <Paper className={classes.paper} elevation={3} square>
            {Array.from(store.records.values()).map((suggestion, index) =>
              renderSuggestion({
                suggestion,
                index,
                itemProps: getItemProps({ item: suggestion }),
                highlightedIndex,
                selectedItem,
              }),
            )}
          </Paper>
        ) : null}
      </div>
    );
  }

  public render() {
    const { classes, error, required, label, id, fieldProps, store, value, addComponent, disabled, onChange } = this.props;

    return (
      <div className={classes.root}>
        <Downshift selectedItem={store.selection} onChange={this.selectionChanged} itemToString={item => item && item.label}>
          {this._renderDownshift(classes, error, required, label, id, fieldProps, store, value, disabled, addComponent)}
        </Downshift>
      </div>
    );
  }
}

const styles = theme => ({
  root: {
    flexGrow: 1,
  },
  container: {
    flexGrow: 1,
    position: 'relative',
  },
  paper: {
    position: 'absolute',
    zIndex: 1,
    marginTop: theme.spacing.unit,
    left: 0,
    right: 0,
  },
  formControl: {
    marginLeft: theme.spacing.unit,
    marginRight: theme.spacing.unit,
    maxWidth: 400
  },
});
export default withStyles(styles as any)<SelectProps>(IntegrationDownshift);
