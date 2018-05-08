import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Button from 'material-ui/Button';

import { UsingContext, UsingType } from './using';

interface SubmitProps {
  text?: string;
  buttonProps?: any;
}

export class Submit extends React.Component<SubmitProps, {}> {

  public render() {
    const { text, buttonProps } = this.props;

    return (
      <UsingContext.Consumer>
        {value => (
          <SubmitConsumer using={value} text={text} buttonProps={buttonProps} />
        )}
      </UsingContext.Consumer>
    );
  }
}

// need to wrap this in observer to make magic happen
const SubmitConsumer = observer((props: SubmitProps & { using: UsingType<any> }) => {
  const { using, text, buttonProps } = props;

  const handleSubmit = () => {
    using.submit();
  };

  return (
    <Button disabled={!using.valid || using.loading} onClick={handleSubmit} {...buttonProps}>{text || 'Save'}</Button>
  );
});
