import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter, Route } from 'react-router-dom'
import Nav from '../common/Nav'
import Snipe from './snipe'
import User from './user'
import Tool from './tool'

const PrimaryLayout = () => (
  <div className="primary-layout">
    <header>
      <Nav items={[
        { text: 'Snipe', href: '/admin/snipe' },
        { text: 'User', href: '/admin/user' },
        { text: 'Tool', href: '/admin/tool' },
      ]} />
    </header>
    <div style={{ position: 'absolute', height: '100%', width: '100%', boxSizing: 'border-box', top: '0px', paddingTop: '85px' }}>
      <div style={{ height: '100%', overflow: 'auto' }}>
        <Route path="/admin/snipe" component={Snipe} />
        <Route path="/admin/user" component={User} />
        <Route path="/admin/tool" component={Tool} />
      </div>
    </div>
  </div>
)

const App = () => (
  <BrowserRouter>
    <PrimaryLayout />
  </BrowserRouter>
)

ReactDOM.render(<App />, document.getElementById('root'))