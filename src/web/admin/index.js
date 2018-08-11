import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter, Route } from 'react-router-dom'
import Nav from '../common/Nav'
import Snipe from './snipe'
import User from './user'

const PrimaryLayout = () => (
  <div className="primary-layout">
    <header>
      <Nav items={[
        {text:'Snipe',href:'/admin/snipe'},
        {text:'User',href:'/admin/user'}
        ]} />
    </header>
    <main>
      <Route path="/admin/snipe" exact component={Snipe} />
      <Route path="/admin/user" component={User} />
    </main>
  </div>
)

const App = () => (
  <BrowserRouter>
    <PrimaryLayout />
  </BrowserRouter>
)

ReactDOM.render(<App />, document.getElementById('root'))