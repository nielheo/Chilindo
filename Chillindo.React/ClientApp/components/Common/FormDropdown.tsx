import * as React from 'react'

import FormInput from './FormInput'

export default class FormDropdown extends React.Component<{ onChange: any, label: string, error: string, disabled: boolean, value: string }, any> {
  public render() {
    //console.log(this.props.airlines)
    return <FormInput label={this.props.label} error={this.props.error} >
      <select
        className="form-control"
        onChange={this.props.onChange}
        disabled={this.props.disabled}
        value={this.props.value}
      >
        {this.props.children}
      </select>
    </FormInput>
  }
}