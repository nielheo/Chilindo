import * as React from 'react';
import { RouteComponentProps } from 'react-router';

import { Grid, Row, Col, Button, Table, Panel } from 'react-bootstrap'
import FormTextbox from './Common/FormTextbox'
import FormDropdown from './Common/FormDropdown'

export class Home extends React.Component<RouteComponentProps<{}>, any> {
  constructor(props: any) {
    super(props)
    this.state = {
      accountNumber: '',
      action: '',
      actionResult: '',
      currency: '',
      amount: 0,
      clicked: false,
      result: null
    }
  }

  _onAccountNumberChange = (e: any) => {
    if (this.state.accountNumber !== e.target.value) {
      this.setState({ accountNumber: e.target.value })
    }
  }

  _onActionChange = (e: any) => {
    if (this.state.action !== e.target.value) {
      this.setState({ action: e.target.value })
    }
  }

  _onCurrencyChange = (e: any) => {
    if (this.state.action !== e.target.value) {
      this.setState({ currency: e.target.value })
    }
  }

  _onAmountChange = (e: any) => {
    let value = parseFloat(e.target.value)
    if (this.state.action !== value) {
      this.setState({ amount: value })
    }
  }

  _validInput = () => {
    if (!this.state.action)
      return false

    if (this.state.action === 'balance' && !this.state.accountNumber)
      return false

    if (this.state.action !== 'balance'
      && (!this.state.accountNumber
          || !this.state.currency
          || (this.state.amount <= 0))
    )
      return false

    return true
  }

  _postTransaction = () => {
    let action = this.state.action
    return fetch('/api/account/' + this.state.accountNumber + '/' + this.state.action, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Accept-Encoding': 'gzip',
      },
      body: JSON.stringify({
        accountNumber: this.state.accountNumber,
        currency: this.state.currency,
        amount: this.state.amount
      })
    }).then(res => {
      if (res) return res.json()
    }).then(res =>
      this.setState({ result: res, actionResult: action })
      ).catch(err => { })
  }

  _submitBalance = () => {
    let action = this.state.action
    return fetch('/api/account/' + this.state.accountNumber + '/balance', {
      method: 'get',
      headers: {
        'Content-Type': 'application/json',
        'Accept-Encoding': 'gzip',
      }
    }).then(res => {
      if (res) return res.json()
    }).then(res =>
      this.setState({ result: res, actionResult: action })
    ).catch(err => { })
  }

  _submitRequest = () => {
    switch (this.state.action.toLowerCase()) {
      case 'balance':
        this._submitBalance()
        break
      case 'deposit':
      case 'withdraw':
        this._postTransaction()
        break
    }
  }

  _onButtonClicked = () => {
    if (this._validInput())
      this._submitRequest()
    else {
      if (!this.state.clicked)
        this.setState({ clicked: true })
    }
  }

  _actionText = (a: string) => {
    switch (a.toLocaleLowerCase()) {
      case 'balance':
        return 'Check Balance'
      case 'deposit':
        return 'Deposit'
      case 'withdraw':
        return 'Withdraw'
      default:
        return ''
    }
  }

  public render() {
    console.log(this.state.result)
    return <section>
      <hr/>
      <Row>
        <Col md={5}>
          
            <Row>
              <Col md={8}>
                <FormTextbox
                  label={'Account Number'}
                  disabled={false}
                  value={this.state.accountNumber}
                  onChange={this._onAccountNumberChange}
                  error={this.state.clicked && !this.state.accountNumber ? '* required' : ''}
                />
              </Col>
            </Row>
            <Row>
              <Col md={8}>
                <FormDropdown
                label={'Action'}
                    error={this.state.clicked && !this.state.action ? '* required' : ''}
                value={this.state.action}
                onChange={this._onActionChange}
                disabled={false}
              >
                  <option value=''>- Select action -</option>
                  <option value='balance'>Check balance</option>
                  <option value='deposit'>Deposit</option>
                  <option value='withdraw'>Withdraw</option>
                </FormDropdown>
              </Col>
            </Row>
            {this.state.action && this.state.action !== 'balance' && <section>
              <Row>
                <Col md={8}>
                  <FormDropdown
                    label={'Currency'}
                  error={this.state.clicked && !this.state.currency ? '* required' : ''}
                    value={this.state.currency}
                    onChange={this._onCurrencyChange}
                    disabled={false}
                  >
                    <option value=''>- Select currency -</option>
                    <option value='CNY'>CNY</option>
                    <option value='MYR'>MYR</option>
                    <option value='THB'>THB</option>
                    <option value='USD'>USD</option>
                    <option value='VND'>VND</option>
                  </FormDropdown>
                </Col>
            </Row>
            <Row>
                <Col md={8}>
                  <FormTextbox
                  label={'Amount'}
                  error={this.state.clicked && parseFloat(this.state.amount) <= 0 ? '* required' : ''}
                  value={this.state.amount}
                  onChange={this._onAmountChange}
                  disabled={false}
                />
              </Col>
            </Row>
            </section>
            }
            {this.state.action && <Button bsStyle='primary' onClick={this._onButtonClicked}>Submit</Button>

            }
        </Col>
        <Col md={7}>
          {this.state.result && this.state.result.successful &&
            <Panel header={'Success - ' + this._actionText(this.state.action)
              + ' (account number: ' + this.state.result.accountNumber + ')'} bsStyle="success">
            {
              this.state.result && this.state.result.accountBalances &&
              <Row><Col md={12}>
                <Table responsive>
                  <thead>
                    <tr>
                      <th>Curr</th>
                      <th>Balance</th>
                    </tr>
                  </thead>
                  <tbody>
                    {this.state.result.accountBalances.map((bal: any) =>
                      <tr>
                        <td>{bal.currency}</td>
                        <td>{bal.balance}</td>

                      </tr>
                    )}
                  </tbody>
                </Table>
              </Col></Row>
            }
            {
              this.state.result && this.state.result.currency &&
              <Row><Col md={12}>
                Final balance {this.state.result.currency} {this.state.result.balance}
              </Col></Row>
            }
            </Panel>
          }
          
          {this.state.result && !this.state.result.successful &&
            <Panel header={'Failed - ' + this._actionText(this.state.action)
              + ' (account number: ' + this.state.result.accountNumber + ')'} bsStyle="danger">
            {this.state.result.message}
            </Panel>
           
          }
        </Col>
      </Row>
    </section>
  }
}
